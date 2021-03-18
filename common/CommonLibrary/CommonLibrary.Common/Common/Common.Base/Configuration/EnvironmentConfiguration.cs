//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Configuration
{
    using Exceptions;
    using CommonLibrary.ServicePlatform.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using System.Text.RegularExpressions;

    /// <summary>The environment configuration.</summary>
    [SettingsSection("Environment")]
    public class EnvironmentConfiguration
    {
        /// <summary>The environment.</summary>
        public enum Environment
        {
            DevLocal,
            Dev,
            Int,
            Prod
        }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the application name.</summary>
        public string ApplicationName { get; set; }

        /// <summary>Gets the full name.</summary>
        public string FullName
        {
            get
            {
                if (this.ApplicationName != null)
                {
                    var match = new Regex(@"fabric:\/(.*)").Match(this.ApplicationName);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }

                return null;
            }
        }

        /// <summary>Gets or sets the absolute path for the service. eg. "/TalentEngagementServiceApp/TalentEngagementService"</summary>
        public string ServiceAbsolutePath { get; set; }

        /// <summary>Gets or sets the absolute uri for the service. eg. "fabric:/TalentEngagementServiceApp/TalentEngagementService"</summary>
        public string ServiceAbsoluteUri { get; set; }

        /// <summary>Gets or sets the cluster. This should be the logical cluster id. i.e: d365-app-cluster-prod-australiaeast.rsu.int.powerapps.com</summary>
        public string Cluster { get; set; }

        public bool IsConsoleApp { get; set; }

        /// <summary>The get global environment json configuration.</summary>
        /// <returns>The <see cref="JsonConfigurationSource"/>.</returns>
        public static JsonConfigurationSource GetGlobalEnvironmentJsonConfiguration()
        {
            return new JsonConfigurationSource
                {
                    Path = "appsettings.json"
                };
        }

        /// <summary>The get environment json configuration.</summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The <see cref="JsonConfigurationSource"/>.</returns>
        public static JsonConfigurationSource GetEnvironmentJsonConfiguration(Environment environment)
        {
            var jsonConfigurationFileName = string.Empty;

            switch (environment)
            {
                case Environment.DevLocal:
                    jsonConfigurationFileName = "appsettings.DevelopmentLocal.json";
                    break;
                case Environment.Dev:
                    jsonConfigurationFileName = "appsettings.Development.json";
                    break;
                case Environment.Int:
                    jsonConfigurationFileName = "appsettings.Integration.json";
                    break;
                case Environment.Prod:
                    jsonConfigurationFileName = "appsettings.Production.json";
                    break;
                default:
                    throw new StartupException($"Unable to determine which json file to load, the environment was unrecognized: {environment}");
                    break;
            }

            return new JsonConfigurationSource
                {
                    Path = jsonConfigurationFileName
                };
        }

        /// <summary>The get environment.</summary>
        /// <returns>The <see cref="Environment"/>.</returns>
        public Environment GetEnvironment()
        {
            switch (this.Name?.ToUpper())
            {
                case "DEVLOCAL":
                    return Environment.DevLocal;
                case "DEV":
                    return Environment.Dev;
                case "INT":
                    return Environment.Int;
                case "PROD":
                    return Environment.Prod;
                default:
                    throw new StartupException($"Unable to determine the environment to load, it was either null or unrecognized: {this.Name}. Please make sure to specify a config section of 'Environment' with a value of 'Name': 'DEVLOCAL/DEV/INT/PROD...ect'");
            }
        }
    }
}
