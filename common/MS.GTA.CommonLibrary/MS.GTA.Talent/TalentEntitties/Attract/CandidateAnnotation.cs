//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class CandidateAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_candidateartifact")]
        public CandidateArtifact CandidateArtifact { get; set; }
    }
}
