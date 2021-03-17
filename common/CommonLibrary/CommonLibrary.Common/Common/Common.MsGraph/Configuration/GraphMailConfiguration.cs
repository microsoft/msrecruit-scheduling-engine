//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.MSGraph.Configuration
{
    using CommonLibrary.ServicePlatform.Configuration;

    /// <summary>
    /// Graph Configuration
    /// </summary>
    [SettingsSection("GraphMailConfiguration")]
    public class GraphMailConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the secret in the key vault to retrieve the scheduler service account user name.
        /// </summary>
        public string GraphEmailSecrets { get; set; }

        /// <summary>
        /// Gets or sets the name of the secret in the key vault to retrieve the scheduler service account password.
        /// </summary>
        public string GraphEmailPasswordSecrets { get; set; }

        /// <summary>
        /// Gets or sets native client app id.
        /// </summary>
        public string NativeClientAppId { get; set; }


        /// <summary>
        /// Gets or sets Graph email secret.
        /// </summary>
        public string GraphEmailsSecret { get; set; }
    }
}
