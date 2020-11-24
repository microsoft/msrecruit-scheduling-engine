//----------------------------------------------------------------------------
// <copyright file="TemplateToken.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data Contract for Template Token
    /// </summary>
    [DataContract]
    public class TemplateToken
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
        /// Gets or sets Token.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false)]
        public string Description { get; set; }

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
        /// Gets or sets a value indicating whether token is being used in template or not.
        /// </summary>
        [DataMember(Name = "inUse", IsRequired = false)]
        public bool InUse { get; set; }

        /// <summary>
        /// Gets or sets token key for Token
        /// </summary>
        [DataMember(Name = "tokenKey", IsRequired = false)]
        public string TokenKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether token is editable or not.
        /// </summary>
        [DataMember(Name = "isEditable", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the Token Id which is referencing the Token at the token pool.
        /// </summary>
        [DataMember(Name = "tokenId", IsRequired = false)]
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets associated token Id for Token Value
        /// </summary>
        public string AssociatedTokenId { get; set; }

        /// <summary>
        /// Gets or sets Ordinal Number.
        /// </summary>
        [DataMember(Name = "ordinalNumber", IsRequired = false)]
        public int OrdinalNumber { get; set; }
    }
}
