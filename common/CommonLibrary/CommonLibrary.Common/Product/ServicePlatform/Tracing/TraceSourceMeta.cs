//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.ServicePlatform.Tracing
{
    /// <summary>
    /// Meta class to used internally by TraceSourceBase
    /// </summary>
    public static class TraceSourceMeta
    {
        private static ILoggerFactory _loggerFactory;

        /// <summary>
        /// Logger Factory
        /// </summary>
        public static ILoggerFactory LoggerFactory {
            get {
                if (_loggerFactory == null)
                {                    
                    throw new NullReferenceException("Logger Factory instance is null.");
                }
                return _loggerFactory;
            }
            set {
                _loggerFactory = value;
            }
        }
    }
}
