//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System.Runtime.Serialization;

    using TA.CommonLibrary.Common.XrmHttp;
    using TA.CommonLibrary.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferNoteAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msgta_jobapplicationoffernote")]
        public JobApplicationOfferNote JobApplicationOfferNote { get; set; }
    }
}
