using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WCA.Web.Pages
{
    [AllowAnonymous]
    public class StatusCodeModel : PageModel
    {
        public StatusCodeModel(ILogger<StatusCodeModel> logger)
        {
            _logger = logger;
        }

        public new int StatusCode { get; set; }
        public string StatusTitle { get; set; }
        public string StatusMessage { get; set; }

        private readonly ILogger<StatusCodeModel> _logger;

        public void OnGet(int statusCode)
        {
            StatusCode = statusCode;

            switch (statusCode)
            {
                case 400:
                    StatusTitle = "Bad request";
                    StatusMessage = "The request cannot be fulfilled due to bad syntax. Some of the data provided may be invalid.";
                    break;
                case 403:
                    StatusTitle = "Forbidden";
                    StatusMessage = "You may not have permissions to access the requested resource.";
                    break;
                case 404:
                    StatusTitle = "Page not found";
                    StatusMessage = "We're sorry, we couldn't find the page that you reqeusted.";
                    break;
                case 408:
                    StatusTitle = "Request Timeout";
                    StatusMessage = "The server timed out waiting for the request.";
                    break;
                case 500:
                    StatusTitle = "Internal Server Error";
                    StatusMessage = "The server was unable to finish processing the request.";
                    break;
                default:
                    StatusTitle = "Unknown error";
                    StatusMessage = "That’s odd... something unexpected happened.";
                    break;
            }

            var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var originalPath = (reExecute == null) ? "" : $", original path: {reExecute.OriginalPath}";
            _logger.LogInformation($"Unexpected status code: {statusCode}{originalPath}");
        }
    }
}
