//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Offer Ruleset
    /// </summary>
    [DataContract]
    public class OfferRuleset
    {
        /// <summary>
        /// Gets or sets Section Name.
        /// </summary>
        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Section Color.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets rulesets.
        /// </summary>
        [DataMember(Name = "fields", IsRequired = false)]
        public IList<OfferRulesetField> Fields { get; set; }
    }
}
