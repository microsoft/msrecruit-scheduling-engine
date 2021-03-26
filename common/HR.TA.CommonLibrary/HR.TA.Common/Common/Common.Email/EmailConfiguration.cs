//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Email
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Email Configuration class
    /// </summary>
    [SettingsSection(nameof(EmailConfiguration))]
    public class EmailConfiguration
    {
        /// <summary>Gets or sets the default domain To Send From. </summary>
        public string DefaultEmailDomainToSendFrom { get; set; }
    }
}
