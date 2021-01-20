// <copyright file="JobOfferTokensDetail.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2.Gdpr
{
    /// <summary>
    /// Gets or sets Token detail
    /// </summary>
    public class JobOfferTokensDetail
    {
        /// <summary>
        /// Gets or sets Token name
        /// </summary>
        public string TokenName { get; set; }

        /// <summary>
        /// Gets or sets Token Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets Default Value
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
