//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Configuration;

namespace CommonLibrary.Common.Product.ServicePlatform.Flighting.AppConfiguration
{
    /// <summary>
    /// Configuration options for the Flighting Api Configuration provider implementation.
    /// </summary>
    [SettingsSection("GTA:ServicePlatform:AppConfigurationFlightConfiguration")]
    public class AppConfigurationFlightConfiguration
    {
        /// <summary>
        /// Get or sets the hosted flighting api url
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets of sets the user application name.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the environment name
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the Client Id to be used to get the app token
        /// </summary>
        public string AppClientId { get; set; }

        /// <summary>
        /// Gets of sets the Key Credential for the Client ID
        /// </summary>
        public string AppKey { get; set; }
    }
}
