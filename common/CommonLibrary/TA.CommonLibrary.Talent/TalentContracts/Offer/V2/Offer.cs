//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V1;
    using TA.CommonLibrary.Common.TalentAttract.Contract;

    /// <summary>
    /// The Offer data contract
    /// </summary>
    [DataContract]
    public class Offer
    {
        /// <summary>
        /// Gets or sets OfferID of model
        /// </summary>
        [DataMember(Name = "offerID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets JobApplication ID of model
        /// </summary>
        [DataMember(Name = "jobApplicationID", IsRequired = false, EmitDefaultValue = false)]
        public string JobApplicationID { get; set; }

        /// <summary>
        /// Gets or sets Candidate name of model
        /// </summary>
        [DataMember(Name = "candidateName", IsRequired = false, EmitDefaultValue = false)]
        public string CandidateName { get; set; }

        /// <summary>
        /// Gets or sets the Candidate object identifier of model, if the candidate is internal.
        /// </summary>
        [IgnoreDataMember]
        public string CandidateOId { get; set; }

        /// <summary>
        /// Gets or sets OfferStatus.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = false)]
        public OfferStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets OfferStatusReason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false, EmitDefaultValue = false)]
        public OfferStatusReason? StatusReason { get; set; }

        /// <summary>
        /// Gets or sets offer comment.
        /// </summary>
        [DataMember(Name = "candidateComment", IsRequired = false, EmitDefaultValue = false)]
        public string CandidateComment { get; set; }

        /// <summary>
        /// Gets or sets offer comment.
        /// </summary>
        [DataMember(Name = "ownerComment", IsRequired = false, EmitDefaultValue = false)]
        public string OwnerComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether required documents
        /// </summary>
        [DataMember(Name = "isSignedDocumentRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsSignedDocumentRequired { get; set; }

        /// <summary>
        /// Gets or sets Offer artifacts.
        /// </summary>
        [DataMember(Name = "offerArtifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferArtifact> OfferArtifacts { get; set; }

        /// <summary>
        /// Gets or sets Offer participants.
        /// </summary>
        [DataMember(Name = "offerParticipants", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferParticipant> OfferParticipants { get; set; }

        /// <summary>
        /// Gets or sets Offer accepted date.
        /// </summary>
        [DataMember(Name = "candidateResponseDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CandidateResponseDate { get; set; }

        /// <summary>
        /// Gets or sets Date when candidate uploads the documents successfully
        /// </summary>
        [DataMember(Name = "candidateDocumentUploadDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CandidateDocumentUploadDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the offer exits multiple version
        /// </summary>
        [DataMember(Name = "isMultipleVersionExist", IsRequired = false, EmitDefaultValue = false)]
        public bool IsMultipleVersionExist { get; set; }

        /// <summary>
        /// Gets or sets previous Offer ID
        /// </summary>
        [DataMember(Name = "previousOfferID", IsRequired = false, EmitDefaultValue = false)]
        public string PreviousOfferID { get; set; }

        /// <summary>
        /// Gets or sets next OFfer ID
        /// </summary>
        [DataMember(Name = "nextOfferID", IsRequired = false, EmitDefaultValue = false)]
        public string NextOfferID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer is locked or not
        /// </summary>
        [DataMember(Name = "isLocked", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsOkToContactCandidate after decline.
        /// </summary>
        [DataMember(Name = "isOkToContactCandidateAfterDecline", IsRequired = false)]
        public bool? IsOkToContactCandidateAfterDecline { get; set; }

        /// <summary>
        /// Gets or sets decline reasons.
        /// </summary>
        [DataMember(Name = "declineReasons", IsRequired = false)]
        public OfferDeclineReason[] DeclineReasons { get; set; }

        /// <summary>
        /// Gets or sets decline comment.
        /// </summary>
        [DataMember(Name = "declineComment", IsRequired = false)]
        public string DeclineComment { get; set; }

        /// <summary>
        /// Gets or sets Offer withdraw date.
        /// </summary>
        [DataMember(Name = "offerWithdrawDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferWithdrawDate { get; set; }

        /// <summary>
        /// Gets or sets job application.
        /// </summary>
        [IgnoreDataMember]
        public V1.Application Application { get; set; }

        /// <summary>
        /// Gets or sets the Job Application Activity
        /// </summary>
        [IgnoreDataMember]
        public string JobApplicationActivity { get; set; }

        /// <summary>
        /// Gets or sets the sections for the offer
        /// </summary>
        [DataMember(Name = "sections", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets Template ID for the offer
        /// </summary>
        [DataMember(Name = "templateID", IsRequired = false, EmitDefaultValue = false)]
        public string TemplateID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the notes for non standard offer
        /// </summary>
        [DataMember(Name = "nonStandardOfferNotes", IsRequired = false, EmitDefaultValue = false)]
        public string NonStandardOfferNotes { get; set; }

        /// <summary>
        /// Gets or sets close offer comment.
        /// </summary>
        [DataMember(Name = "closeOfferComment", IsRequired = false)]
        public string CloseOfferComment { get; set; }

        /// <summary>
        /// Gets or sets OfferStatusReason before closing offer.
        /// </summary>
        [DataMember(Name = "statusReasonBeforeClose", IsRequired = false, EmitDefaultValue = false)]
        public OfferStatusReason? StatusReasonBeforeClose { get; set; }

        /// <summary>
        /// Gets or sets Offer close date.
        /// </summary>
        [DataMember(Name = "offerCloseDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferCloseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer is sequential approval or not.
        /// </summary>
        [DataMember(Name = "isSequentialApprovalRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsSequentialApprovalRequired { get; set; }

        /// <summary>
        /// Gets or sets Offer expiration date.
        /// </summary>
        [DataMember(Name = "offerExpirationDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Required Candidate Documents.
        /// </summary>
        [DataMember(Name = "requiredCandidateDocuments", IsRequired = false, EmitDefaultValue = false)]
        public List<string> RequiredCandidateDocuments { get; set; }

        /// <summary>
        /// Gets or sets TokenValues
        /// </summary>
        [DataMember(Name = "tokenValues", IsRequired = false, EmitDefaultValue = false)]
        public IList<TokenValue> TokenValues { get; set; }

        /// <summary>
        /// Gets or sets templatePackageID for the offer
        /// </summary>
        [DataMember(Name = "templatePackageID", IsRequired = false, EmitDefaultValue = false)]
        public string TemplatePackageID { get; set; }

        /// <summary>
        /// Gets or sets Template Package Name
        /// </summary>
        [DataMember(Name = "templatePackageName", IsRequired = false, EmitDefaultValue = false)]
        public string TemplatePackageName { get; set; }

        /// <summary>
        /// Gets or sets offer Package Documents.
        /// </summary>
        [DataMember(Name = "offerPackageDocuments", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferPackageDocument> OfferPackageDocuments { get; set; }

        /// <summary>
        /// Gets or sets Offer Publish date.
        /// </summary>
        [DataMember(Name = "offerPublishDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferPublishDate { get; set; }

        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        [IgnoreDataMember]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the e sign type used.
        /// </summary>
        [DataMember(Name = "esignTypeUsed", IsRequired = false, EmitDefaultValue = true)]
        public ESignType ESignTypeUsed { get; set; }

        /// <summary>
        /// Gets or sets offer created date.
        /// </summary>
        [DataMember(Name = "createdDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the graph object identifier of the user who created the offer
        /// </summary>
        [DataMember(Name = "createdByOid", IsRequired = false, EmitDefaultValue = false)]
        public string CreatedByOid { get; set; }

        /// <summary>
        /// Gets or sets offer updated date.
        /// </summary>
        [DataMember(Name = "updatedDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the graph object identifier of the user who updated the offer
        /// </summary>
        [DataMember(Name = "updatedByOid", IsRequired = false, EmitDefaultValue = false)]
        public string UpdatedByOid { get; set; }

        /// <summary>
        /// Gets or sets the Offer Notes for the Job Application
        /// </summary>
        [DataMember(Name = "offerNotes", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferNote> OfferNotes { get; set; }

        /// <summary>
        /// Gets or sets the Optional tokens in the offer
        /// </summary>
        [DataMember(Name = "optionalTokens", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> OptionalTokens { get; set; }

        /// <summary>
        /// Gets or sets the current author participant id of the Offer
        /// </summary>
        [DataMember(Name = "currentAuthorParticipantId", IsRequired = false, EmitDefaultValue = false)]
        public string OfferCurrentAuthorParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the author who created non standard notes
        /// </summary>
        [DataMember(Name = "NonStandardNotesCreatedBy", IsRequired = false, EmitDefaultValue = false)]
        public string NonStandardNotesCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the current author participant of the Offer
        /// </summary>
        [IgnoreDataMember]
        public OfferParticipant OfferCurrentAuthorParticipant { get; set; }
    }
}
