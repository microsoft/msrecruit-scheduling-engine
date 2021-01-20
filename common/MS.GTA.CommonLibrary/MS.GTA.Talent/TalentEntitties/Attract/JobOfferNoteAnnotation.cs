//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferNoteAnnotation.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System.Runtime.Serialization;

    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferNoteAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msgta_jobapplicationoffernote")]
        public JobApplicationOfferNote JobApplicationOfferNote { get; set; }
    }
}
