//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2.Gdpr
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
