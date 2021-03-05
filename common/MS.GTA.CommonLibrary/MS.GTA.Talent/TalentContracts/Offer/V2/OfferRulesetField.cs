//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Offer Ruleset Field
    /// </summary>
    [DataContract]
    public class OfferRulesetField
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
        /// Gets or sets token value for field
        /// </summary>
        [DataMember(Name = "value", IsRequired = false)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets token display text for field
        /// </summary>
        [DataMember(Name = "displayText", IsRequired = false)]
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets data type of token.
        /// </summary>
        [DataMember(Name = "dataType", IsRequired = false)]
        public OfferTemplateRulesetFieldType? DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token is Overridden or not
        /// </summary>
        [DataMember(Name = "isOverridden", IsRequired = false)]
        public bool IsOverridden { get; set; }
    }
}
