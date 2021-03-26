//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace TA.CommonLibrary.Common.Base.Utilities
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A wrapper logger factory that does nothing when disposed.
    /// </summary>
    public class LoggerFactoryWithoutDispose : ILoggerFactory
    {
        /// <summary>
        /// The Logger Factory
        /// </summary>
        private ILoggerFactory realLoggerFactory;

        /// <summary>
        /// Assigns value depending up on call to dispose method
        /// </summary>
        private bool wouldHaveBeenDisposed;

        /// <summary>
        /// The Logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerFactoryWithoutDispose"/> class.
        /// </summary>
        /// 
        public LoggerFactoryWithoutDispose()
        {
            this.realLoggerFactory = new LoggerFactory();
            this.logger = this.realLoggerFactory.CreateLogger<LoggerFactoryWithoutDispose>();
        }

        /// <inheritdoc/>
        public void AddProvider(ILoggerProvider provider)
        {
            this.realLoggerFactory.AddProvider(provider);
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            if (wouldHaveBeenDisposed)
            {
                this.logger.LogWarning($"CreateLogger called after Dispose was called! { System.Environment.StackTrace}");
            }

            return this.realLoggerFactory.CreateLogger(categoryName);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.logger.LogInformation($"Dispose LoggerFactoryWithoutDispose - doing nothing! {System.Environment.StackTrace}");
            wouldHaveBeenDisposed = true;
        }
    }
}
