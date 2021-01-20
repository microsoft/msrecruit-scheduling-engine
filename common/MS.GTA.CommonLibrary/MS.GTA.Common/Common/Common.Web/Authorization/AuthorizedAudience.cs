﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AuthorizedAudience.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.Authorization
{
    using System.Collections.Generic;
    using System.Linq;
    using MS.GTA.Common.Base;
    using MS.GTA.ServicePlatform.Configuration;  

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
