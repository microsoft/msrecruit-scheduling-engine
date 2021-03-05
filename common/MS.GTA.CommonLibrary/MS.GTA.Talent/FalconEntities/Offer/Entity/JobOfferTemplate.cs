//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace Common.Provisioning.Entities.FalconEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using Common.DocumentDB.Contracts;
    using System.Collections.Generic;

    [DataContract]
    public class JobOfferTemplate : DocDbEntity
    {
        [DataMember(Name = "JobOfferTemplateID")]
        public string JobOfferTemplateID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "TemplateUri")]
        public string TemplateUri { get; set; }

        [DataMember(Name = "AccessLevel")]
        public JobOfferTemplateAccessLevel AccessLevel { get; set; }

        [DataMember(Name = "Status")]
        public JobOfferTemplateStatus Status { get; set; }

        [DataMember(Name = "StatusReason")]
        public JobOfferTemplateStatusReason StatusReason { get; set; }

        [DataMember(Name = "Owner")]
        public string Owner { get; set; }

        [DataMember(Name = "ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "ValidTo")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "Participants")]
        public IList<JobOfferTemplateParticipant> Participants { get; set; }

        [DataMember(Name = "Sections")]
        public IList<JobOfferTemplateSection> Sections { get; set; }

        [DataMember(Name = "JobOfferTemplateId")]
        public string JobOfferTemplateIdentifier { get; set; }

        [DataMember(Name="Tokens")]
        public IList<string> Tokens { get; set; }

        [DataMember(Name= "OriginalTemplateID")]
        public string OriginalTemplateID { get; set; }

        [DataMember(Name= "IsOfferTextEditable")]
        public bool IsOfferTextEditable { get; set; }

        [DataMember(Name = "PreviousTemplateID")]
        public string PreviousTemplateID { get; set; }

        [DataMember(Name = "NextTemplateID")]
        public string NextTemplateID { get; set; }

        [DataMember(Name = "ActivatedDate")]
        public DateTime? ActivatedDate { get; set; }

        [DataMember(Name = "Content")]
        public string Content { get; set; }
    }
}
