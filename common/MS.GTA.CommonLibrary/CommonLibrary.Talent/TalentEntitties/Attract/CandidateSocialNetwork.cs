//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract
{
    using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Optionset;
    using CommonLibrary.Common.XrmHttp;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text;


    [ODataEntity(PluralName = "msdyn_candidatesocialnetworks", SingularName = "msdyn_candidatesocialnetwork")]
    public class CandidateSocialNetwork : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidatesocialnetworkid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "msdyn_socialnetwork")]
        public SocialNetwork? SocialNetwork { get; set; }

        [DataMember(Name = "msdyn_memberreference")]
        public string MemberReference { get; set; }

        [DataMember(Name = "msdyn_socialnetworkidentity")]
        public string SocialNetworkIdentity { get; set; }
    }
}