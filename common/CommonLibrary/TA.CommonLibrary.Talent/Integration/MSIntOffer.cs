//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Integration.Contracts
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// The MSIntOffer object
  /// </summary>
  public class MSIntOffer
  {
    public string OfferID { get; set; }

    public string PreviousOfferID { get; set; }

    public string NextOfferID { get; set; }

    public string JobApplicationID { get; set; }

    public string ExternalJobApplicationID { get; set; }

    public string JobOpeningID { get; set; }

    public string ExternalJobOpeningID { get; set; }

    public string ExternalSource { get; set; }

    public string OfferPrepareDeepLinkUri { get; set; }

    public string OfferCandidateDeepLinkUri { get; set; }

    public string OfferStatus { get; set; }

    public string OfferStatusReason { get; set; }

    public string OfferOwnerComment { get; set; }

    public DateTime? OfferPublishDate { get; set; }

    public DateTime? OfferWithdrawDate { get; set; }

    public DateTime? OfferCloseDate { get; set; }

    public DateTime? OfferExpirationDate { get; set; }

    public DateTime? OfferAcceptedDate { get; set; }

    public string NonStandardOfferNotes { get; set; }

    public string CloseOfferComment { get; set; }

    public string OfferStatusReasonBeforeClose { get; set; }

    public bool? IsSequentialApprovalRequired { get; set; }

    public List<string> RequiredCandidateDocuments { get; set; }

    public string CandidateEmail { get; set; }

    public string CandidateExternalID { get; set; }

    public string CandidateComment { get; set; }

    public bool? IsOkToContactCandidateAfterDecline { get; set; }

    public DateTime? CandidateResponseDate { get; set; }

    public DateTime? CandidateDocumentUploadDate { get; set; }

    public IEnumerable<string> OfferDeclineReason { get; set; }

    public string OfferDeclineComment { get; set; }

    public List<MSIntArtifact> OfferArtifacts { get; set; }

    public List<MSIntParticipant> OfferParticipants { get; set; }

    public List<MSIntToken> Tokens { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedByEmail { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedByEmail { get; set; }
  }
}
