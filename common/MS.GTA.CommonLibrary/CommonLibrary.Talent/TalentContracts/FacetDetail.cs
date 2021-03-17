//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Facet Details contract.
    /// </summary>
    [DataContract]
    public class FacetDetail
    {
        /// <summary>
        /// Facet Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        /// <summary> 
        /// Facet selection boolean.
        /// </summary>
        [DataMember(Name = "isSelected", EmitDefaultValue = false, IsRequired = false)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// facet count.
        /// </summary>
        [DataMember(Name = "count", EmitDefaultValue = false, IsRequired = false)]
        public int Count { get; set; }

        /// <summary>
        /// facet id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false, IsRequired = false)]
        public string Id { get; set; }
    }
}
