//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Product.ServicePlatform.Instrumentation.ApplicationInsights
{
    using Microsoft.Extensions.Logging;
    using TA.CommonLibrary.CommonDataService.Common.Internal;

    /// <summary>
    /// The <see cref="AppInsightsLoggerProvider"/> defines the mechanism to provision the <see cref="ILogger"/> instances.
    /// </summary>
    /// <seealso cref="ILoggerProvider" />
    public class AppInsightsLoggerProvider : ILoggerProvider
    {
        private readonly string instrumentationKey = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInsightsLoggerProvider"/> class.
        /// </summary>
        /// <param name="iKey">The instrumentation key.</param>
        public AppInsightsLoggerProvider(string iKey)
        {
            Contract.CheckNonEmpty(iKey, nameof(iKey));
            this.instrumentationKey = iKey;
        }

        /// <summary>
        /// Creates a new <see cref="ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The instance for <see cref="ILogger"/>.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            Contract.CheckValue(categoryName, nameof(categoryName));
            return new AppInsightsLogger(categoryName, this.instrumentationKey);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do Nothing
        }
    }
}
