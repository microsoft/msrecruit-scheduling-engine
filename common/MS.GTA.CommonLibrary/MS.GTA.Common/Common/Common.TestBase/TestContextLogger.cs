// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TestContextLogger.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase
{
    using System;

    using Microsoft.Extensions.Logging;
    using MS.GTA.CommonDataService.Instrumentation;
    using MS.GTA.ServicePlatform.Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>The test context logger.</summary>
    public class TestContextLogger : ILogger
    {
        private readonly TestContext testContext;

        public TestContextLogger(TestContext testContext)
        {
            this.testContext = testContext;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return default(IDisposable);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Ignore activity and metric data for test debugging.
            if (logLevel <= LogLevel.Information
                && (state is ActivityLogData || state is MetricLogData))
            {
                return;
            }

            string message = formatter(state, exception);

            // Ignore unneccessary log messages for test debugging.
            if (message == "Parent activity is null. Adding new RootActivityContext.")
            {
                return;
            }

            this.testContext?.WriteLine(message);
        }
    }

    public class TestContextLogProvider : ILoggerProvider
    {
        private readonly TestContext testContext;

        public TestContextLogProvider(TestContext testContext)
        {
            this.testContext = testContext;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestContextLogger(this.testContext);
        }
    }
}
