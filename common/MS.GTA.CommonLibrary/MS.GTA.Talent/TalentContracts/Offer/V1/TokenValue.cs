//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Tokens
    /// </summary>
    [DataContract]
    public class TokenValue : Token
    {
        /// <summary>
        /// Gets or sets value of Token
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets value of DisplayText
        /// </summary>
        [DataMember(Name = "displayText")]
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether token is selected or only part of a ruleset
        /// </summary>
        [DataMember(Name = "isSelected")]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets default value
        /// </summary>
        [DataMember(Name = "defaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token can be Overridden or not
        /// </summary>
        [DataMember(Name = "isOverridden", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsOverridden { get; set; }

        /// <summary>
        /// Gets or sets entity value
        /// </summary>
        [DataMember(Name = "entityValue")]
        public string EntityValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token is Hidden or not
        /// </summary>
        [DataMember(Name = "isDeleted", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets value id of Token
        /// </summary>
        [IgnoreDataMember]
        public string ValueId { get; set; }
    }
}