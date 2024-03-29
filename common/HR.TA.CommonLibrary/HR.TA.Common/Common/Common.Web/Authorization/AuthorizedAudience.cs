//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Web.Authorization
{
    using System.Collections.Generic;
    using System.Linq;
    using HR.TA.Common.Base;
    using HR.TA.ServicePlatform.Configuration;  

    /// <summary>
    /// authorized audiences class
    /// </summary>
    [SettingsSection("AuthorizedAudience")]
    public class AuthorizedAudience
    {
        /// <summary>
        /// Gets or sets app token valid audiences
        /// </summary>
        public string AppTokenValidAudiences { get; set; }

        /// <summary>
        /// Gets gets or sets app token valid audiences list
        /// </summary>
        public IList<string> AppTokenValidAudiencesList => this.SplitToList(this.AppTokenValidAudiences);

        /// <summary>
        /// Gets or sets user token valid audiences
        /// </summary>
        public string UserTokenValidAudiences { get; set; }

        /// <summary>
        /// Gets gets or sets user token valid audiences list
        /// </summary>
        public IList<string> UserTokenValidAudiencesList => this.SplitToList(this.UserTokenValidAudiences);

        private IList<string> SplitToList(string valuesAsSingleString)
        {
            var values = new List<string>();

            if (!string.IsNullOrWhiteSpace(valuesAsSingleString))
            {
                values.AddRange(valuesAsSingleString.Split(Constants.SplitCharacter).Select(v => v.Trim()));
            }

            return values;
        }
    }
}
