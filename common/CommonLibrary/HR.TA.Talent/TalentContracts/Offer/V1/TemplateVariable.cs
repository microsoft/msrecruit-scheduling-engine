//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Template Variables
    /// </summary>
    [DataContract]
    public class TemplateVariable
    {
        /// <summary>
        /// Gets or sets Category.
        /// </summary>
        [DataMember(Name = "category", IsRequired = true)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Token.
        /// </summary>
        [DataMember(Name = "token", IsRequired = true)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        [DataMember(Name = "value", IsRequired = false)]
        public string Value { get; set; }
    }
}
