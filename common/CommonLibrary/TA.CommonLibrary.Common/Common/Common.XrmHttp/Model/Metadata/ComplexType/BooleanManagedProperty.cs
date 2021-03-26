//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    // TODO: use definition from SDK???
    public class BooleanManagedProperty
    {
        [DataMember(Name = "Value")]
        public bool? Value { get; set; }
        
        [DataMember(Name = "CanBeChanged")]
        public bool? CanBeChanged { get; set; }
        
        [DataMember(Name = "ManagedPropertyLogicalName")]
        public string ManagedPropertyLogicalName { get; set; }
    }
}
