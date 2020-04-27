using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace WCA.Actionstep.Client.Extensions
{
    public static class ServiceBuilderExtensions
    {
        public static IHttpClientBuilder AddActionstepService(
            this IServiceCollection serviceCollection,
            ProductInfoHeaderValue productInfoHeaderValue,
            Action<ActionstepServiceConfigurationOptions> configureActionstepServiceConfigurationOptions)
        {
            if (configureActionstepServiceConfigurationOptions is null) throw new ArgumentNullException(nameof(configureActionstepServiceConfigurationOptions));

            var actionstepServiceConfigurationOptions = new ActionstepServiceConfigurationOptions();
            configureActionstepServiceConfigurationOptions.Invoke(actionstepServiceConfigurationOptions);

            return serviceCollection
                .AddTransient<AuthDelegatingHandler>()
                .AddSingleton(actionstepServiceConfigurationOptions)
                .AddHttpClient<IActionstepService, ActionstepService>(c =>
                {
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ActionstepService.ActionstepAcceptMediaType));
                    c.DefaultRequestHeaders.UserAgent.Add(productInfoHeaderValue);
                })
                .AddHttpMessageHandler<AuthDelegatingHandler>();
        }
    }
}
