using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleExceptionHandling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using WCA.Actionstep.Client;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Conveyancing.WorkspaceCreation;
using WCA.Core.Features.FirstTitle.Connection;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Features.Pexa.Authentication;
using WCA.Core.Services;
using WCA.Domain.Actionstep;
using WCA.FirstTitle.Client;
using WCA.GlobalX.Client.Authentication;
using WCA.PEXA.Client.Resources;
using WCA.Web.Areas.API;

namespace WCA.Web.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> logger;

        // TODO: Really need to get App Insights working with ILogger.
        //       This is a quick and nasty to make progress.
        private readonly ITelemetryLogger telemetryLogger;

        private readonly IWebHostEnvironment env;

        public ApiExceptionFilter(
            ILogger<ApiExceptionFilter> logger,
            ITelemetryLogger telemetryLogger,
            IWebHostEnvironment env)
        {
            this.logger = logger;
            this.telemetryLogger = telemetryLogger;
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.HttpContext.Request.Path.StartsWithSegments(new PathString("/api"), StringComparison.InvariantCultureIgnoreCase))
            {
                // Don't handle if we're not under the /api path
                return;
            }

            string message = string.Empty;
            Guid errorReferenceId = Guid.NewGuid();

            if (env.IsDevelopment())
            {
                message = context.Exception.Message;
            }
            else
            {
                message = "Unfortunately an error has occurred. Please try again, or contact support@workcloud.com.au with a brief description of what you were trying to do, as well as the following error ID:" + errorReferenceId.ToString();
            }

            telemetryLogger.TrackException(
                context.Exception,
                new Dictionary<string, string>()
                {
                    { "Logging Source", "ApiExceptionFilter" },
                    { "WCA Error ID", errorReferenceId.ToString() }
                });

            var handlingResult =
                Handling.Prepare()
                    .On<ValidationException>((ex) =>
                    {
                        logger.LogError(0, ex, "ValidationException has been thrown. Error Id:" + errorReferenceId.ToString());
                        message = "Validation Failed";
                        context.Result = new BadRequestObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.BadRequest };
                    })
                    .On<InfoTrackRequestFailedException>((ex) =>
                    {
                        logger.LogError(0, ex, "InfoTrackRequestFailedException has been thrown. Error Id:" + errorReferenceId.ToString());
                        message = ex.Message;
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.BadGateway };
                    })
                    .On<HttpRequestException>((ex) =>
                    {
                        logger.LogError(0, ex, "HttpRequestException has been thrown. Error Id:" + errorReferenceId.ToString());
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.InternalServerError };
                    })
                    .On<InvalidOperationException>((ex) =>
                    {
                        logger.LogError(0, ex, "InvalidOperationException has been thrown. Error Id:" + errorReferenceId.ToString());
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.InternalServerError };
                    })
                    .On<UnauthorizedAccessException>((ex) =>
                    {
                        logger.LogError(0, ex, "UnauthorizedAccessException has been thrown. Error Id:" + errorReferenceId.ToString());
                        if (context.Exception.Message.ToUpperInvariant().Contains("TOKEN", StringComparison.InvariantCulture)
                        && context.Exception.Message.ToUpperInvariant().Contains("EXPIRED", StringComparison.InvariantCulture))
                        {
                            if (context.Exception.Message.ToUpperInvariant().Contains("ACTIONSTEP", StringComparison.InvariantCulture))
                            {
                                message = "Your access token has expired. Please logout and login again to actionstep";
                            }
                            else
                            {
                                message = "Your access token has expired.";
                            }
                        }
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<NotImplementedException>((ex) =>
                    {
                        logger.LogCritical(0, ex, "NotImplementedException has been thrown. Error Id:" + errorReferenceId.ToString());
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.NotImplemented };
                    })
                    .On<TimeoutException>((ex) =>
                    {
                        logger.LogError(0, ex, "TimeoutException has been thrown. Error Id:" + errorReferenceId.ToString());
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.GatewayTimeout };
                    })
                    .On<InvalidCredentialsForActionstepApiCallException>((ex) =>
                    {
                        logger.LogError(ex, "Could not find valid credentials to connect to the Actionstep API.");
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"ActionstepConnection OrgKey={ex.ActionstepOrgKey}");
                        context.HttpContext.Response.Headers.Add("Warning", "InvalidCredentials");
                        context.Result = new ObjectResult(new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<InvalidTokenSetException>((ex) =>
                    {
                        logger.LogError(ex, "Unable to refresh access token.");
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"ActionstepConnection OrgKey={ex?.TokenSet?.OrgKey}");
                        context.HttpContext.Response.Headers.Add("Warning", "InvalidTokenSet");
                        context.Result = new ObjectResult(new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<PexaBadRequestResponseException>((ex) =>
                    {
                        logger.LogError(ex, "There was a problem with the submitted request to PEXA");

                        var pexaException = (PEXAException)ex.InnerException;
                        var exceptionResponse = pexaException?.ExceptionResponse;

                        context.ModelState.Clear();
                        foreach (var exceptionItem in exceptionResponse.ExceptionList)
                        {
                            context.ModelState.AddModelError(exceptionItem.Code, exceptionItem.Message);
                        }

                        context.Result = new ObjectResult(new ErrorViewModel(message, context.ModelState)) { StatusCode = (int)HttpStatusCode.BadRequest };
                    })
                    .On<PexaUnexpectedErrorResponseException>((ex) =>
                    {
                        var pexaException = (PEXAException)ex.InnerException;
                        logger.LogError("Encountered unexpected HTTP exception during PEXA workspace creation", WCASeverityLevel.Error,
                            new Dictionary<string, string> {
                                { "statusCode", pexaException.StatusCode.ToString(CultureInfo.InvariantCulture) },
                                { "errorResponse", pexaException.Response },
                        });

                        context.Result = new ObjectResult(new ErrorViewModel(message, context.ModelState)) { StatusCode = (int)HttpStatusCode.InternalServerError };
                    })
                    .On<InvalidCredentialsForInfoTrackException>((ex) =>
                    {
                        message = "There was a problem connecting to InfoTrack.";
                        logger.LogError(ex, "There was a problem connecting to InfoTrack.");
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"InfoTrackConnection OrgKey={ex.ActionstepOrgKey}");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<InfoTrackCredentialsNotFoundException>((ex) =>
                    {
                        message = "InfoTrack is not configured for your organisation.";
                        logger.LogError(ex, message);
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"InfoTrackConnection OrgKey={ex.ActionstepOrgKey}");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<MissingOrInvalidPexaApiTokenException>((ex) =>
                    {
                        message = "You don't have an active PEXA connection.";
                        logger.LogError(ex, message);
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"PexaConnection");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<InvalidGlobalXApiTokenException>((ex) =>
                    {
                        message = "You don't have an active GlobalX connection.";
                        logger.LogError(ex, message);
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"GlobalXConnection");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<MissingFirstTitleCredentialsException>((ex) =>
                    {
                        message = "You don't have an active First Title connection.";
                        logger.LogError(ex, message);
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"FirstTitleConnection");
                        context.HttpContext.Response.Headers.Add("Warning", "MissingCredentials");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<InvalidFirstTitleCredentialsException>((ex) =>
                    {
                        message = "You don't have an active First Title connection.";
                        logger.LogError(ex, message);
                        context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"FirstTitleConnection");
                        context.HttpContext.Response.Headers.Add("Warning", "InvalidCredentials");
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.Unauthorized };
                    })
                    .On<MatterNotFoundException>((ex) =>
                    {
                        message = "Could not find the matter specified.";
                        logger.LogError(ex, message);
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.NotFound };
                    })
                    .On<CannotCreatePexaWorkspaceException>((ex) =>
                    {
                        message = ex.Message;
                        logger.LogError(ex, message);
                        context.Result = new ObjectResult(
                            new ErrorViewModel(message, context.ModelState))
                        { StatusCode = (int)HttpStatusCode.NotFound };
                    })
                    .Catch(context.Exception, throwIfNotHandled: false);

            if (!handlingResult.Handled)
            {
                logger.LogError(0, context.Exception, "Unknown exception has been thrown. Error Id:" + errorReferenceId.ToString());
                context.Result = new ObjectResult(
                    new ErrorViewModel(message, context.ModelState))
                { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

        }
    }
}