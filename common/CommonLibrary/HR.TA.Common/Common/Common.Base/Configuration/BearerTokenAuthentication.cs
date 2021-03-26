//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{
    using ServicePlatform.Configuration;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Bearer token authentication class
    /// </summary>
    [SettingsSection("BearerTokenAuthentication")]
    public class BearerTokenAuthentication
    {
        /// <summary>
        /// Gets or sets audience URL
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets authority URL
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets valid audiences 
        /// </summary>
        public string ValidAudiences { get; set; }

        public IList<string> ValidAudiencesList => SplitToList(ValidAudiences);

        private IList<string> SplitToList(string valuesAsSingleString)
        {
            var values = new List<string>();

            if (!string.IsNullOrWhiteSpace(valuesAsSingleString))
            {
                values.AddRange(valuesAsSingleString.Split(Base.Constants.SplitCharacter).Select(v => v.Trim()));
            }

            return values;
        }
    }
}

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BapEnvironmentConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace HR.TA.Common.Cdm.Configuration
{
}

