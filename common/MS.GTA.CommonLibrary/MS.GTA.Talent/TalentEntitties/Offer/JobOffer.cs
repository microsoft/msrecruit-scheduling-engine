//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;

    [ODataEntity(PluralName = "msdyn_joboffers", SingularName = "msdyn_joboffer")]
    public class JobOffer : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string AutoNumber { get; set; }

        [DataMember(Name = "msdyn_versionno")]
        public int? VersionNo { get; set; }

        [DataMember(Name = "_msdyn_jobapplication_value")]
        public Guid? JobApplicationRecId { get; set; }

        [DataMember(Name = "msdyn_jobapplication")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationactivity_value")]
        public Guid? JobApplicationActivityRecId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationactivity")]
        public JobApplicationActivity JobApplicationActivity { get; set; }

        [DataMember(Name = "msdyn_offeracceptdate")]
        public DateTime? OfferAcceptDate { get; set; }

        [DataMember(Name = "msdyn_candidateresponsedate")]
        public DateTime? CandidateResponseDate { get; set; }

        [DataMember(Name = "msdyn_offerpublishdate")]
        public DateTime? OfferPublishDate { get; set; }

        [DataMember(Name = "msdyn_offerwithdrawdate")]
        public DateTime? OfferWithdrawDate { get; set; }

        [DataMember(Name = "msdyn_offerexpirationdate")]
        public DateTime? OfferExpirationDate { get; set; }

        [DataMember(Name = "msdyn_validfrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "msdyn_validto")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "msdyn_offerclosedate")]
        public DateTime? OfferCloseDate { get; set; }

        [DataMember(Name = "msdyn_candidatedocumentuploaddate")]
        public DateTime? CandidateDocumentUploadDate { get; set; }

        [DataMember(Name = "msdyn_candidatecomment")]
        public string CandidateComment { get; set; }

        [DataMember(Name = "msdyn_declinecomment")]
        public string DeclineComment { get; set; }

        [DataMember(Name = "msdyn_ownercomment")]
        public string OwnerComment { get; set; }

        [DataMember(Name = "msdyn_withdrawcomment")]
        public string WithdrawComment { get; set; }

        [DataMember(Name = "msdyn_closeoffercomment")]
        public string CloseOfferComment { get; set; }

        [DataMember(Name = "msdyn_withdrawcandidatenotificationsubject")]
        public string WithdrawCandidateNotificationSubject { get; set; }

        [DataMember(Name = "msdyn_withdrawcandidatenotificationbody")]
        public string WithdrawCandidateNotificationBody { get; set; }

        [DataMember(Name = "msdyn_nonstandardoffernotes")]
        public string NonStandardOfferNotes { get; set; }

        [DataMember(Name = "msdyn_offerlocale")]
        public string OfferLocale { get; set; }

        [DataMember(Name = "msdyn_requiredcandidatedocuments")]
        public string RequiredCandidateDocuments { get; set; }

        [DataMember(Name = "msdyn_issigneddocumentrequired")]
        public bool? IsSignedDocumentRequired { get; set; }

        [DataMember(Name = "msdyn_issequentialapprovalrequired")]
        public bool? IsSequentialApprovalRequired { get; set; }

        [DataMember(Name = "msdyn_issenttocandidate")]
        public bool? IsSentToCandidate { get; set; }

        [DataMember(Name = "msdyn_islocked")]
        public bool? IsLocked { get; set; }

        [DataMember(Name = "msdyn_offercurrentauthorparticipant")]
        public JobOfferParticipant OfferCurrentAuthorParticipant { get; set; }

        [DataMember(Name = "_msdyn_offercurrentauthorparticipant_value")]
        public Guid? OfferCurrentAuthorParticipantRecId { get; set; }

        [DataMember(Name = "msdyn_isoktocontactcandidateafterdecline")]
        public bool? IsOkToContactCandidateAfterDecline { get; set; }

        [DataMember(Name = "msdyn_withdrawcandidatenotification")]
        public bool? WithdrawCandidateNotification { get; set; }

        [DataMember(Name = "msdyn_isnonstandardoffer")]
        public bool? IsNonStandardOffer { get; set; }

        [DataMember(Name = "msdyn_statusreasonbeforeclose")]
        public JobOfferStatusReason? StatusReasonBeforeClose { get; set; }

        [DataMember(Name = "msdyn_nonstandardofferreason")]
        public JobOfferNonStandardReason? NonStandardOfferReason { get; set; }

        [DataMember(Name = "msdyn_declinereasons")]
        public string DeclineReasons { get; set; }

        [DataMember(Name = "msdyn_isexpiryextended")]
        public bool? IsExpiryUpdated { get; set; }

        [DataMember(Name = "msdyn_joboffertemplateid")]
        public JobOfferTemplate JobOfferTemplateID { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplateid_value")]
        public Guid? JobOfferTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_previousversion")]
        public JobOffer PreviousVersion { get; set; }

        [DataMember(Name = "_msdyn_previousversion_value")]
        public Guid? PreviousVersionRecid { get; set; }

        [DataMember(Name = "msdyn_nextversion")]
        public JobOffer NextVersion { get; set; }

        [DataMember(Name = "_msdyn_nextversion_value")]
        public Guid? NextVersionRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferstatusid")]
        public JobOfferStatus JobOfferStatus { get; set; }

        [DataMember(Name = "_msdyn_jobofferstatusid_value")]
        public Guid? JobOfferStatusRecId { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplatepackageid_value")]
        public Guid? JobOfferTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatepackageid")]
        public JobOfferTemplatePackage JobOfferTemplatePackage { get; set; }

        [DataMember(Name = "msdyn_joboffer_jobofferapprovalemail")]
        public IList<JobOfferApprovalEmail> JobOfferApprovalEmails { get; set; }
        
        [DataMember(Name = "msdyn_joboffer_jobofferpackagedocument")]
        public IList<JobOfferPackageDocument> JobOfferPackageDocuments { get; set; }

        [DataMember(Name = "msdyn_joboffer_joboffertokenvalue")]
        public IList<JobOfferTokenValue> JobOfferTokenValues { get; set; }

        [DataMember(Name = "msdyn_joboffer_jobofferartifact")]
        public IList<JobOfferArtifact> Artifacts { get; set; }

        [DataMember(Name = "msdyn_joboffer_joboffersection")]
        public IList<JobOfferSection> Sections { get; set; }

        [DataMember(Name = "msdyn_joboffer_jobofferparticipant")]
        public IList<JobOfferParticipant> Participant { get; set; }

        [DataMember(Name = "msdyn_nonstandardoffernotescreatedby")]
        public Worker NonStandardOfferNotesCreatedBy { get; set; }

        [DataMember(Name = "msdyn_activetemplatepackageid")]
        public JobOfferTemplatePackage ActiveTemplatePackage { get; set; }

        [DataMember(Name = "_msdyn_activetemplatepackageid_value")]
        public Guid? ActiveTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_joboffer_optionaljoboffertoken")]
        public IList<JobOfferToken> OptionalTokens { get; set; }
    }
}