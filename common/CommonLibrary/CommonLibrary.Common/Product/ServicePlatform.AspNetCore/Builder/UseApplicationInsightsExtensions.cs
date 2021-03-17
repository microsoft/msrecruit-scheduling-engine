//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Product.ServicePlatform.AspNetCore.Builder
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using CommonLibrary.Common.Common.Common.Base.Configuration;
    using CommonLibrary.ServicePlatform.Configuration;

    public static class UseApplicationInsightsExtensions
    {
        /// <summary>
        /// Sets the Telemetry Configuration for the Application
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> reference of the application</param>
        /// <param name="configurationManager"> The <see cref="IConfigurationManager"/> instance of the application</param>
        /// <param name="configuration">The instnce for <see cref="IConfiguration"/>.</param>
        /// <returns></returns>
        public static IApplicationBuilder SetTelemetryConfiguration(this IApplicationBuilder builder, IConfigurationManager configurationManager, IConfiguration configuration)
        {
            var telemetryConfiguration = builder.ApplicationServices.GetRequiredService<TelemetryConfiguration>();
            var loggingConfiguration = configurationManager.Get<LoggingConfiguration>();
            telemetryConfiguration.InstrumentationKey = loggingConfiguration.InstrumentationKey;
            //builder.AddInternalTelemetryInitializers(configuration);
            return builder;
        }

        /// <summary>
        /// Adds Telemetry service to the services collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance of the services collection</param>        
        /// <param name="configuration">The instance for <see cref="IConfiguration"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddTelemetryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry();
            //services.AddAspNetCorrelationProvider(configuration);
            return services;
        }
    }
}
