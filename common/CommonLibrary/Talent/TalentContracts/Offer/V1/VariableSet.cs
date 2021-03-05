//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Variables Sets
    /// </summary>
    [DataContract]
    public class VariableSet
    {
        /// <summary>
        /// Gets or sets Token.
        /// </summary>
        [DataMember(Name = "token", IsRequired = true)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets Pattern.
        /// </summary>
        [DataMember(Name = "pattern", IsRequired = false)]
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        [DataMember(Name = "value", IsRequired = false)]
        public string Value { get; set; }
    }
}
