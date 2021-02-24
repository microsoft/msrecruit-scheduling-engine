//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TestBase.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.CommonDataService.Instrumentation;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.Common.Base.KeyVault;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.Routing;
    using MS.GTA.Common.Routing.Constants;
    using MS.GTA.Common.Routing.DocumentDb;
    using MS.GTA.Common.TestBase.Contracts;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Security;
    using MS.GTA.ServicePlatform.Tracing;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Moq;

    using AAD = ServicePlatform.Azure.AAD;
    using MS.GTA.ServicePlatform.Azure.Security;

    public static class RunnerServiceUtils
    {
        private const string MetricName = "Result";
        private const string TestDimension = "Test";
        private const string ResultDimension = "Result";
        private const string Failure = "Failed";
        private const string Success = "Passed";

        /// <summary>
        /// The Hcm global document client used in runner service
        /// </summary>
        /// <param name="configurationManager">The configuaration manager</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <returns></returns>
        public static async Task<DocumentClientGenerator> GetRunnerDocumentClient(IConfigurationManager configurationManager, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            var certificateManager = new CertificateManager();
            var azureActiveDirectoryClient = new AAD.AzureActiveDirectoryClient(configurationManager, certificateManager);
            var environmentNameConfiguration = configurationManager.Get<EnvironmentNameConfiguration>();
            var storageConfig = GlobalServiceEnvironmentSettings.GetGlobalDocumentDbStorageConfiguration(environmentNameConfiguration.EnvironmentName);

            var mockRoutingClient = new Mock<IRoutingClient>();
            mockRoutingClient.Setup(m => m.GetGlobalDocumentDbStorageConfiguration()).Returns(storageConfig);

            var secretManagerProvider = new SecretManager(azureActiveDirectoryClient, configurationManager);
            return new DocumentClientGenerator(
              mockRoutingClient.Object,
              null,
              secretManagerProvider,
              loggerFactory.CreateLogger<DocumentClientGenerator>(),
              new DocumentClientStore(new MemoryCache(new MemoryCacheOptions())), configurationManager);
        }

        /// <summary>
        /// Runner Tests
        /// </summary>
        /// <param name="environmentIdForRunnerService">The configuaration manager</param>
        /// <param name="runnerServiceName">The trace source</param>
        /// <param name="documentClientGenerator">The document client generator</param>
        /// <param name="retryIntervalInMinutes">retry Interval in minutes</param>
        /// <param name="logger">The logger</param>
        /// <param name="runTests">run tests</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public static async Task GetRunnerService(
            string environmentIdForRunnerService,
            string runnerServiceName,
            IDocumentClientGenerator documentClientGenerator,
            int retryIntervalInMinutes,
            ILogger logger,
            Func<Task> runTests,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckValue(runnerServiceName, nameof(runnerServiceName));
            Contract.CheckValue(documentClientGenerator, nameof(documentClientGenerator));
            Contract.CheckValue(environmentIdForRunnerService, nameof(environmentIdForRunnerService));
            Contract.CheckValue(logger, nameof(logger));

            var documentClient = await documentClientGenerator.GetHcmGlobalDocumentClient();
            var hcmDocumentClient = new HcmDocumentClient(documentClient, StorageConfigurationSettings.HCMDatabaseId, Constants.RunnerSettings);
            var runnerSettings = await hcmDocumentClient.Get<RunnerSettings>(environmentIdForRunnerService);
            var runnerServiceConfiguration = runnerSettings?.RunnerServices.Where(r => r.RunnerServiceName == runnerServiceName).FirstOrDefault();
            var isRunnerEnabled = runnerServiceConfiguration?.IsRunnerEnabled;
            logger.LogInformation($"{runnerServiceName}Service.RunAsync(): IsRunnerEnabled is set to {isRunnerEnabled}");

            if (isRunnerEnabled == true)
            {
                retryIntervalInMinutes = runnerServiceConfiguration.RetryIntervalInMinutes;
                logger.LogInformation($"{runnerServiceName}Service.RunAsync(): Started running tests");

                try
                {
                    await runTests();
                }
                catch (Exception e)
                {
                    logger.LogError($"Exception in RunAsync: {e.Message} \n InnerException: {e.InnerException}");
                }
            }

            logger.LogInformation($"{runnerServiceName}: Will wait {retryIntervalInMinutes} minutes before next run");
            await Task.Delay(TimeSpan.FromMinutes(retryIntervalInMinutes), cancellationToken);
        }

        /// <summary>
        /// Executes a single runner test and ensures the right metrics are captured
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="runnerServiceName">The runner service</param>
        /// <param name="testName">The name of the test to run</param>
        /// <param name="testToRun">The func to execute the test</param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task ExecuteRunnerTestScenario(
            ILogger logger,
            string runnerServiceName,
            string testName,
            Func<Task<bool>> testToRun)
        {
            Contract.CheckValue(runnerServiceName, nameof(runnerServiceName));
            Contract.CheckValue(logger, nameof(logger));

            var rootActivityId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var runIdentifier = $"{runnerServiceName}.{testName}.RunnerTest";

            await logger.ExecuteRoot(
                            new RootExecutionContext
                            {
                                SessionId = sessionId,
                                RootActivityId = rootActivityId,
                            },
                            "ExecuteRunnerTestScenario",
                            async () =>
                            {
                                logger.LogInformation($"{runIdentifier} running with sessionId: {sessionId} and rootActivityId: {rootActivityId}");
                                var stopwatch = new Stopwatch();
                                stopwatch.Start();

                                var runStatus = Failure;

                                try
                                {
                                    var result = await testToRun();
                                    if (result)
                                    {
                                        runStatus = Success;
                                    }
                                }
                                catch (Exception e)
                                {
                                    logger.LogError($"{runIdentifier}: Test error: {e}!");
                                }

                                stopwatch.Stop();
                                logger.LogInformation($"Test result for {runIdentifier} : {runStatus}");
                                logger.LogMetric(
                                    MetricName,
                                    new SortedList<string, string>() {
                                                 { TestDimension, runIdentifier },
                                                 { ResultDimension, runStatus } },
                                    stopwatch.ElapsedMilliseconds);
                            });

        }

        public static async Task RunnerStartup(
            IConfigurationManager configurationManager,
            ILoggerFactory loggerFactory,
            string runnerServiceName,
            Func<Task<bool>> runTests,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckValue(runnerServiceName, nameof(runnerServiceName));
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("RunnerServiceUtils");

            await logger.ExecuteAsync(
                "StartupRunner",
                async () =>
                {
                    int retryIntervalInMinutes = 15;

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var documentClientGenerator = await RunnerServiceUtils.GetRunnerDocumentClient(configurationManager, loggerFactory);
                            var runnerConfiguration = configurationManager.Get<RunnerServiceConfiguration>();

                            while (!cancellationToken.IsCancellationRequested)
                            {
                                await RunnerServiceUtils.GetRunnerService(
                                     runnerConfiguration.EnvironmentIdForRunnerService,
                                     runnerServiceName,
                                     documentClientGenerator,
                                     retryIntervalInMinutes,
                                     logger,
                                     async () =>
                                     {
                                         var stopwatch = new Stopwatch();
                                         stopwatch.Start();

                                         var result = await runTests();

                                         stopwatch.Stop();

                                         var resultString = result ? Success : Failure;

                                         logger.LogMetric(MetricName, new SortedList<string, string>() {
                                                 { TestDimension, $"{runnerServiceName}.IntegrationTests" },
                                                 { ResultDimension, resultString } }, stopwatch.ElapsedMilliseconds);
                                     });
                                await Task.Delay(TimeSpan.FromMinutes(retryIntervalInMinutes), cancellationToken);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.LogCritical($"Unhandled exception occured in runner orchestration, {e}");
                        }

                        logger.LogInformation($"{runnerServiceName}: Will wait 5 minutes before next run");
                        // We should never hit this as our runner orchestrator should never fail.
                        await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
                    }
                });
        }

        /// <summary>
        /// Gets the Cluster Uri from the global service
        /// </summary>
        /// <param name="configurationManager">The Configuration Manager</param>
        /// <param name="environmentIdForRunnerService">The environment Id for runner</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="trace">The trace</param>
        /// <returns>cluster uri</returns>
        public static async Task<string> GetClusterName(
            IConfigurationManager configurationManager,
            string environmentIdForRunnerService,
            ILoggerFactory loggerFactory,
            ITraceSource trace)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckValue(environmentIdForRunnerService, nameof(environmentIdForRunnerService));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(trace, nameof(trace));

            var environmentName = configurationManager.Get<EnvironmentNameConfiguration>().EnvironmentName;
            string serviceBaseUri = string.Empty;

            if (!environmentName.Equals(Constants.LocalEnvName, StringComparison.OrdinalIgnoreCase))
            {
                var clusterUri = await TokenExtraction.GetClusterUri(configurationManager, loggerFactory, environmentIdForRunnerService, environmentName, trace);
                serviceBaseUri = clusterUri;
            }
            else
            {
                serviceBaseUri = Constants.LocalServiceBaseURI;
            }

            trace.TraceInformation($"Runner.RunTests(): ClusterUri: {serviceBaseUri}!");

            return serviceBaseUri;
        }
    }
}
