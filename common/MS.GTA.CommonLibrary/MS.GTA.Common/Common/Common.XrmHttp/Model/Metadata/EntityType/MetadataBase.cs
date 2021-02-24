//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataNamespace(Namespace = "Microsoft.Dynamics.CRM")]
    public class MetadataBase : ODataEntity
    {
        [Key]
        [DataMember(Name = "MetadataId")]
        public Guid? MetadataId { get; set; }

        [DataMember(Name = "HasChanged")]
        public bool? HasChanged { get; set; }
    }
}
