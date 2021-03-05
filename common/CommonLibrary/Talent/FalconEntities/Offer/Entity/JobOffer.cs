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
    public class JobOffer : DocDbEntity
    {
        [DataMember(Name = "JobOfferID")]
        public string JobOfferID { get; set; }

        [DataMember(Name = "JobApplication")]
        public string JobApplication { get; set; }

        [DataMember(Name = "JobApplicationActivity")]
        public string JobApplicationActivity { get; set; }

        [DataMember(Name = "OfferAcceptDate")]
        public DateTime? OfferAcceptDate { get; set; }

        [DataMember(Name = "CandidateResponseDate")]
        public DateTime? CandidateResponseDate { get; set; }

        [DataMember(Name = "OfferPublishDate")]
        public DateTime? OfferPublishDate { get; set; }

        [DataMember(Name = "OfferWithdrawDate")]
        public DateTime? OfferWithdrawDate { get; set; }

        [DataMember(Name = "OfferExpirationDate")]
        public DateTime? OfferExpirationDate { get; set; }

        [DataMember(Name = "IsSignedDocumentRequired")]
        public bool? IsSignedDocumentRequired { get; set; }

        [DataMember(Name = "IsSequentialApprovalRequired")]
        public bool? IsSequentialApprovalRequired { get; set; }

        [DataMember(Name = "Status")]
        public JobOfferStatus Status { get; set; }

        [DataMember(Name = "DeclineReasons")]
        public IList<JobOfferDeclineReason> DeclineReasons { get; set; }

        [DataMember(Name = "StatusReason")]
        public JobOfferStatusReason StatusReason { get; set; }

        [DataMember(Name = "ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "ValidTo")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "CandidateComment")]
        public string CandidateComment { get; set; }

        [DataMember(Name = "DeclineComment")]
        public string DeclineComment { get; set; }

        [DataMember(Name = "OwnerComment")]
        public string OwnerComment { get; set; }

        [DataMember(Name = "WithdrawComment")]
        public string WithdrawComment { get; set; }

        [DataMember(Name = "IsSentToCandidate")]
        public bool? IsSentToCandidate { get; set; }

        [DataMember(Name = "PreviousVersion")]
        public string PreviousVersion { get; set; }

        [DataMember(Name = "NextVersion")]
        public string NextVersion { get; set; }

        [DataMember(Name = "Artifacts")]
        public IList<JobOfferArtifact> Artifacts { get; set; }

        [DataMember(Name = "IsLocked")]
        public bool? IsLocked { get; set; }

        [DataMember(Name = "IsOkToContactCandidateAfterDecline")]
        public bool? IsOkToContactCandidateAfterDecline { get; set; }

        [DataMember(Name = "WithdrawCandidateNotification")]
        public bool? WithdrawCandidateNotification { get; set; }

        [DataMember(Name = "WithdrawCandidateNotificationSubject")]
        public string WithdrawCandidateNotificationSubject { get; set; }

        [DataMember(Name = "WithdrawCandidateNotificationBody")]
        public string WithdrawCandidateNotificationBody { get; set; }

        [DataMember(Name = "Sections")]
        public IList<JobOfferSection> Sections { get; set; }

        [DataMember(Name = "JobOfferTemplateID")]
        public string JobOfferTemplateID { get; set; }

        [DataMember(Name = "CloseOfferComment")]
        public string CloseOfferComment { get; set; }

        [DataMember(Name = "StatusReasonBeforeClose")]
        public JobOfferStatusReason StatusReasonBeforeClose { get; set; }

        [DataMember(Name = "NonStandardOfferNotes")]
        public string NonStandardOfferNotes { get; set; }

        [DataMember(Name = "NonStandardOfferReason")]
        public JobOfferNonStandardReason NonStandardOfferReason { get; set; }

        [DataMember(Name = "IsNonStandardOffer")]
        public bool IsNonStandardOffer { get; set; }

        [DataMember(Name = "OfferCloseDate")]
        public DateTime? OfferCloseDate { get; set; }

        [DataMember(Name = "OfferLocale")]
        public string OfferLocale { get; set; }

        [DataMember(Name = "RequiredCandidateDocuments")]
        public string RequiredCandidateDocuments { get; set; }

        [DataMember(Name = "TokenValues")]
        public IList<JobOfferTokenValue> TokenValues { get; set; }

        [DataMember(Name = "IsExpiryUpdated")]
        public bool IsExpiryUpdated { get; set; }

        [DataMember(Name = "CandidateDocumentUploadDate")]
        public DateTime? CandidateDocumentUploadDate { get; set; }        

        [DataMember(Name = "JobOfferTemplatePackageID")]
        public string JobOfferTemplatePackageID { get; set; }

        [DataMember(Name = "JobOfferPackageDocuments")]
        public IList<JobOfferPackageDocument> JobOfferPackageDocuments { get; set; }

        [DataMember(Name = "OptionalTokens")]
        public IList<string> OptionalTokens { get; set; }

        [DataMember(Name = "OfferCurrentAuthorParticipantId")]
        public string OfferCurrentAuthorParticipantId { get; set; }

        [DataMember(Name = "NonStandardOfferNotesCreatedById")]
        public string NonStandardOfferNotesCreatedById { get; set; }

        [DataMember(Name = "ActiveTemplatePackageId")]
        public string ActiveTemplatePackageId { get; set; }
    }
}
