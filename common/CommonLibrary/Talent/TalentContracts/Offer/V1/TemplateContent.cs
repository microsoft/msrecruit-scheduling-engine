//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Variables Sets
    /// </summary>
    [DataContract]
    public class TemplateContent
    {
        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        [DataMember(Name = "content", IsRequired = true)]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets Variable Sets.
        /// </summary>
        [DataMember(Name = "variableSets", IsRequired = true)]
        public IList<VariableSet> VariableSets { get; set; }
    }
}
