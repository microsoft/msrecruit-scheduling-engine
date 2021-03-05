//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Attract
{
    using System.Runtime.Serialization;

    using Common.XrmHttp;
    using Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferNoteAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msgta_jobapplicationoffernote")]
        public JobApplicationOfferNote JobApplicationOfferNote { get; set; }
    }
}
