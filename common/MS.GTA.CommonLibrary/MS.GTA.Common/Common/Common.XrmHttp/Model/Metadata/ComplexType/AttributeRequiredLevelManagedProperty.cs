//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class AttributeRequiredLevelManagedProperty
    {
        [DataMember(Name = "Value")]
        public AttributeRequiredLevel? Value { get; set; }
        
        [DataMember(Name = "CanBeChanged")]
        public bool? CanBeChanged { get; set; }
        
        [DataMember(Name = "ManagedPropertyLogicalName")]
        public string ManagedPropertyLogicalName { get; set; }
    }
}
