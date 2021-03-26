//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Offer Token
    /// </summary>
    [DataContract]
    public class OfferToken
    {
        /// <summary>
        /// Gets or sets Id of the token
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets token type.
        /// </summary>
        [DataMember(Name = "tokenType", IsRequired = false)]
        public TokenType TokenType { get; set; }

        /// <summary>
        /// Gets or sets data type of token.
        /// </summary>
        [DataMember(Name = "dataType", IsRequired = false)]
        public TokenDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets token key for Token
        /// </summary>
        [DataMember(Name = "tokenKey", IsRequired = false)]
        public string TokenKey { get; set; }

        /// <summary>
        /// Gets or sets token value for Token
        /// </summary>
        [DataMember(Name = "tokenValue", IsRequired = false)]
        public string TokenValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token can be Editable or not
        /// </summary>
        [DataMember(Name = "isEditable", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Token can be Overridden or not
        /// </summary>
        [DataMember(Name = "isOverridden", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsOverridden { get; set; }

        /// <summary>
        /// Gets or sets associated token Id for Token Value
        /// </summary>
        public string AssociatedTokenId { get; set; }
    }
}
