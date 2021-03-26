//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA..Common.Provisioning.Entities.XrmEntities.Attract
{
    using System.Runtime.Serialization;

    using HR.TA..Common.XrmHttp;
    using HR.TA..Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferNoteAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msgta_jobapplicationoffernote")]
        public JobApplicationOfferNote JobApplicationOfferNote { get; set; }
    }
}
