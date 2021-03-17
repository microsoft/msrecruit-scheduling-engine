//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.MsGraph.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// The B2C bearer token authentication config settings.
    /// </summary>
    [SettingsSection(nameof(BearerTokenAuthenticationB2C))]
    public class BearerTokenAuthenticationB2C
    {
        /// <summary>
        /// Gets or sets the audience setting.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the authority setting.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets the SignInOrSignUpPolicy setting.
        /// </summary>
        public string SignInOrSignUpPolicy { get; set; }
    }
}
