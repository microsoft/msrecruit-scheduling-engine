//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace CommonLibrary.Common.Base.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Configuration file.
    /// </summary>
    [SettingsSection("BearerTokenAuthenticationForB2CTests")]
    public sealed class BearerTokenAuthenticationForB2CTests
    {
        /// <summary>Gets or sets the environment name.</summary>
        public string EnvironmentName { get; set; }

        /// <summary>Gets or sets the key vault secret name.</summary>
        public string SecretName { get; set; }

        /// <summary>Gets or sets the audience test authentication.</summary>
        public string Audience { get; set; }

        /// <summary>Gets or sets the issuer test authentication.</summary>
        public string Issuer { get; set; }
    }
}
