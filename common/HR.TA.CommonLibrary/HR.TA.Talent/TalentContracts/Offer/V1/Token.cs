//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Tokens
    /// </summary>
    [DataContract]
    public class Token
    {
        /// <summary>
        /// Gets or sets Token Id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Token Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Token Description
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Token Type
        /// </summary>
        [DataMember(Name = "tokenType")]
        public TokenType TokenType { get; set; }

        /// <summary>
        /// Gets or sets Token Data Type
        /// </summary>
        [DataMember(Name = "dataType")]
        public TokenDataType DataType { get; set; }

        /// <summary>
        ///  Gets or sets Section Name of Token
        /// </summary>
        [DataMember(Name = "sectionName")]
        public string SectionName { get; set; }

        /// <summary>
        ///  Gets or sets Section Id of Token
        /// </summary>
        [DataMember(Name = "sectionId")]
        public string SectionId { get; set; }

        /// <summary>
        ///  Gets or sets List of templates in which this token is used
        /// </summary>
        [DataMember(Name = "templates")]
        public IList<string> Templates { get; set; }

        /// <summary>
        ///  Gets or sets Created Date Time of Token
        /// </summary>
        [DataMember(Name = "createdDate", IsRequired = false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets ruleset id of a ruleset token
        /// </summary>
        [DataMember(Name = "rulesetId", IsRequired = false, EmitDefaultValue = false)]
        public string RulesetId { get; set; }

        /// <summary>
        /// Gets or sets ruleset version id of a ruleset token
        /// </summary>
        [DataMember(Name = "rulesetVersionId", IsRequired = false, EmitDefaultValue = false)]
        public string RulesetVersionId { get; set; }

        /// <summary>
        /// Gets or sets ruleset details of a ruleset token
        /// </summary>
        [DataMember(Name = "rulesetDetails")]
        public IList<RulesetDetail> RulesetDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token can be Editable or not
        /// </summary>
        [DataMember(Name = "isEditable", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token is updated or not
        /// </summary>
        [DataMember(Name = "isUpdated", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsUpdated { get; set; }
    }
}
