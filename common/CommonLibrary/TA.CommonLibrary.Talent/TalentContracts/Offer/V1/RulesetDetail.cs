//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Ruleset Details of a token
    /// </summary>
    [DataContract]
    public class RulesetDetail
    {
        /// <summary>
        /// Gets or sets Token Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Token Hierarchy Level
        /// </summary>
        [DataMember(Name = "level")]
        public int Level { get; set; }
    }
}
