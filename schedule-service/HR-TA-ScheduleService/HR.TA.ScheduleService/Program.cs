//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using HR.TA.ServicePlatform.Tracing;

    /// <summary>
    /// Main method
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory();
            TraceSourceMeta.LoggerFactory = loggerFactory;
            var logger = loggerFactory.CreateLogger<Program>();
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the web host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Web Host Builder</returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<StartupConsole>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);
    }
}
