//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ECSConfigurationProvider.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Microsoft.Skype.ECS.Client;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Configuration;
using MS.GTA.ServicePlatform.Context;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MS.GTA.ServicePlatform.Flighting.ECSProvider
{
    /// <summary>
    /// A flights provider implementation of the Experimentation and Configuration Service.
    /// </summary>
    /// <remarks>
    /// The class is meant to be consumed as a singleton service.
    public sealed class ECSConfigurationProvider : FlightsProvider<IDictionary<string, string>>
    {
        private readonly ECSProviderConfiguration configuration;
        private const string environmentRequestOption = "Environment";
        private const string settingConfigurationRootPath = "";
        private IECSConfigurationRequester _ecsConfigRequester;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ECSConfigurationProvider> logger;

        /// <summary>
        /// Creates a new instance of <see cref="ECSConfigurationProvider"/> given an implementation of
        /// <see cref="IConfigurationManager"/>. The provided <see cref="configurationManager"/> needs
        /// to be able to supply the <see cref="ECSProviderConfiguration"/> when requested during
        /// initialization.
        /// </summary>
        public ECSConfigurationProvider(IConfigurationManager configurationManager, ILogger<ECSConfigurationProvider> logger)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));

            configuration = configurationManager.Get<ECSProviderConfiguration>();
            this.logger = logger;
            InitializeConfigurationRequester();
        }

        /// <summary>
        /// Creates a new instance of <see cref="ECSConfigurationProvider"/> given a <see cref="ECSProviderConfiguration"/>.
        /// </summary>
        public ECSConfigurationProvider(ECSProviderConfiguration configuration, ILogger<ECSConfigurationProvider> logger)
        {
            Contract.CheckValue(configuration, nameof(configuration));

            this.configuration = configuration;
            this.logger = logger;
            InitializeConfigurationRequester();
        }

        /// <summary>
        /// Determines whether the provided flight is universally enabled or not.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        public bool IsEnabled(string flight)
        {
            Contract.CheckValue(flight, nameof(flight));

            var flightObject = new RuntimeFlight(flight);
            return this.IsEnabled(flightObject);
        }

        /// <summary>
        /// Determines whether the provided flight is universally enabled or not.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        public override bool IsEnabled(Flight flight)
        {
            Contract.CheckValue(flight, nameof(flight));

            return ActivityLoggerExtensions.Execute(
                this.logger,
                CheckFeatureActivityType.Instance,
                () =>
                {
                    var universallyEnabledFeatures = this.GetEnabledNames().ToList();
                    return universallyEnabledFeatures.Contains(flight.Name);
                });
        }

        /// <summary>
        /// Determines whether the provided flight is enabled or not for provided context.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        /// /// <exception cref="ArgumentNullException">Thrown when the <paramref name="evaluationContext"/> argument is null.</exception>
        public override bool IsEnabled(Flight flight, IDictionary<string, string> evaluationContext)
        {
            Contract.CheckValue(flight, nameof(flight));
            Contract.CheckValue(evaluationContext, nameof(evaluationContext));

            return ActivityLoggerExtensions.Execute(
                this.logger,
                CheckFeatureActivityType.Instance,
                () =>
                {
                    var enabledFeatures = this.GetEnabledNames(evaluationContext).ToList();
                    return enabledFeatures.Contains(flight.Name);
                });
        }

        [Obsolete("This method is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        public override bool IsEnabled<TContext>(Flight flight, TContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gives all universally enabled flights.
        /// </summary>
        /// <returns>Returns a collection of flights that are universally enabled.</returns>
        protected override IEnumerable<string> GetEnabledNames()
        {
            return ActivityLoggerExtensions.Execute(
                this.logger,
                GetEnabledFeaturesActivityType.Instance,
                () =>
                {
                    var requestIds = new Dictionary<string, string>();
                    requestIds.Add(environmentRequestOption, this.configuration.EnvironmentName);
                    return FetchFeaturesListFromECS(requestIds);
                });
        }

        /// <summary>
        /// Gives all enabled flights for the provided context.
        /// </summary>
        /// <returns>Returns a collection of flights that are enabled.</returns>
        protected override IEnumerable<string> GetEnabledNames(IDictionary<string, string> evaluationContext)
        {
            Contract.CheckValue(evaluationContext, nameof(evaluationContext));

            return ActivityLoggerExtensions.Execute(
                this.logger,
                GetEnabledFeaturesActivityType.Instance,
                () =>
                {
                    if (!evaluationContext.ContainsKey(environmentRequestOption))
                    {
                        evaluationContext.Add(environmentRequestOption, this.configuration.EnvironmentName);
                    }
                    return FetchFeaturesListFromECS(evaluationContext.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                });
        }

        [Obsolete("This method is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        protected override IEnumerable<string> GetEnabledNames<TContext>(TContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches and parses the features configuration from ECS Service.
        /// </summary>
        /// <param name="requestIds">Dictionary of parameters to be passed to ECS service</param>
        /// <returns>List of enabled features</returns>
        private List<string> FetchFeaturesListFromECS(Dictionary<string, string> requestIds)
        {
            List<string> lstEnabledFeatures = new List<string>();
            try
            {
                var ecsSettings = FetchFeatureSettings(requestIds);
                string features = ecsSettings.GetValue(settingConfigurationRootPath);
                if (!string.IsNullOrWhiteSpace(features))
                {
                    var json = JObject.Parse(features);
                    foreach (var item in json)
                    {
                        if (ecsSettings.GetValue<bool>(string.Format(this.configuration.FeatureConfigurationTemplate, item.Key)))
                            lstEnabledFeatures.Add(item.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while reading the configuration in {nameof(FetchFeaturesListFromECS)} method, defaulting to zero flights enabled. Error Details: {ex.Message}.");
            }

            return lstEnabledFeatures;
        }

        /// <summary>
        /// Calls the ECS Service and fetches the configuration settings.
        /// </summary>
        /// <param name="requestIds">Dictionary of parameters to be passed to ECS service</param>
        /// <returns></returns>
        private IECSConfigSettings FetchFeatureSettings(Dictionary<string, string> requestIds)
        {
            var settingsETag = _ecsConfigRequester.GetSettings(requestIds).GetAwaiter().GetResult();
            return settingsETag.Settings;
        }

        /// <summary>
        /// Singleton initialization of the ECS Configuration Requester
        /// </summary>
        private void InitializeConfigurationRequester()
        {
            Contract.CheckValue(configuration, nameof(configuration));
            Contract.CheckValue(configuration.ClientName, nameof(configuration.ClientName));
            Contract.CheckValue(configuration.ProjectTeamName, nameof(configuration.ProjectTeamName));

            ActivityLoggerExtensions.Execute(
                logger,
                InitializeProviderActivityType.Instance,
                () =>
                {                    
                    // Be sure to call Initialize only once! It triggers calls to the flight control service.
                    _ecsConfigRequester = configuration.EnableCaching ? ECSClientFactory.CreateLocal() : ECSClientFactory.CreateRemote();
                    ECSClientConfiguration clientConfiguration = new ECSClientConfiguration((EnvironmentType)configuration.EnvironmentType, configuration.ClientName, configuration.ProjectTeamName);
                    var initTask = _ecsConfigRequester.Initialize(clientConfiguration);

                    if (!initTask.Wait(configuration.InitializationTimeout))
                            throw new ECSProviderInitializationTimedOutException(configuration.InitializationTimeout);                    
                });
        }
    }
}
