// <copyright file="CommonLogger.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.Base.Utilities
{
    using MS.GTA.ServicePlatform.Tracing;
    using Microsoft.Extensions.Logging;
    using System.Collections.Concurrent;

    /// <summary>The common logger.</summary>
    public static class CommonLogger
    {
        /// <summary>The cached loggers.</summary>
        private static ConcurrentDictionary<string, ILogger> cachedLoggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// Common Logger
        /// </summary>
        static CommonLogger()
        {
            Logger = TraceSourceMeta.LoggerFactory.CreateLogger(nameof(CommonLogger));
        }
        
        /// <summary>
        /// The logger for common
        /// </summary>
        public static ILogger Logger { get; set; }

        /// <summary>The get a cached logger allowing you to have a custom logger name without custom inline code.</summary>
        /// <param name="loggerName">The logger name.</param>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public static ILogger GetLogger(string loggerName = nameof(CommonLogger))
        {
            ILogger logger;

            cachedLoggers.TryGetValue(loggerName, out logger);

            if (logger == null)
            {
                logger = TraceSourceMeta.LoggerFactory.CreateLogger(loggerName);
                cachedLoggers.TryAdd(loggerName, logger);
            }

            return logger;
        }
    }
}
