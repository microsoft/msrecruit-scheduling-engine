//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.RelevanceSearch
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class RelevanceSearchHighlight
    {
        [DataMember(Name = "entityType")]
        public string EntityType { get; set; }

        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "valueWithHighlight")]
        public string ValueWithHighlight { get; set; }

        [DataMember(Name = "id")]
        public Guid Id { get; set; }
    }
}