//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Product.ServicePlatform.Instrumentation.ApplicationInsights
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The <see cref="AppInsightsLoggerFactoryExtensions"/> defines the extension methods for <see cref="ILoggerFactory"/>.
    /// </summary>
    public static class AppInsightsLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds the application insights logger provider.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        /// <returns></returns>
        public static ILoggerFactory AddAppInsightsLoggerProvider(this ILoggerFactory factory, string instrumentationKey)
        {
            factory.AddProvider(new AppInsightsLoggerProvider(instrumentationKey));
            return factory;
        }
    }
}
