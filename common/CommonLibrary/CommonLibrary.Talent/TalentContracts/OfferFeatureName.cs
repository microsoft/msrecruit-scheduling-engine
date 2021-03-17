//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for offer feature names
    /// </summary>
    [DataContract]
    public enum OfferFeatureName
    {
        /// <summary>
        /// Approval Required
        /// </summary>
        ApprovalRequired,

        /// <summary>
        /// Approval Comment Required
        /// </summary>
        ApprovalCommentRequired,

        /// <summary>
        /// Sequential Approval 
        /// </summary>
        SequentialApproval,

        /// <summary>
        /// Parallel Approval
        /// </summary>
        ParallelApproval,

        /// <summary>
        /// Decline Offer
        /// </summary>
        DeclineOffer,

        /// <summary>
        /// Offer expiration date
        /// </summary>
        OfferExpirationDate,

        /// <summary>
        /// Custom Redirect Url
        /// </summary>
        CustomRedirectUrl,
        
        /// <summary>
        /// Onboarding Redirect Required
        /// </summary>
        OnboardingRedirectRequired
    }
}
