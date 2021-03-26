//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for feature names
    /// </summary>
    [DataContract]
    public enum Feature
    {
        /// <summary>
        /// Job template
        /// </summary>
        JobTemplate,

        /// <summary>
        /// Job posting
        /// </summary>
        JobPosting,

        /// <summary>
        /// Assessment
        /// </summary>
        Assessment,

        /// <summary>
        /// Offer management
        /// </summary>
        OfferManagement,

        /// <summary>
        /// Prospect
        /// </summary>
        Prospect,

        /// <summary>
        /// Skype Interview
        /// </summary>
        SkypeInterview,

        /// <summary>
        /// Candidate reccomendation
        /// </summary>
        CandidateReccomendation,

        /// <summary>
        /// Job reccomendation
        /// </summary>
        JobReccomendation,

        /// <summary>
        /// Talent companion bot
        /// </summary>
        TalentCompanion,

        /// <summary>
        /// Position management
        /// </summary>
        PositionManagement,

        /// <summary>
        /// Custom activities
        /// </summary>
        CustomActivies,

        /// <summary>
        /// Broadbean Integration
        /// </summary>
        BroadbeanIntegration,

        /// <summary>
        /// Dashboard
        /// </summary>
        Dashboard,

        /// <summary>
        /// Job Approval
        /// </summary>
        JobApproval,

        /// <summary>
        /// Analytics
        /// </summary>
        Analytics,

        /// <summary>
        /// Email-template
        /// </summary>
        EmailTemplate,

        /// <summary>
        /// SearchEngineOptimization
        /// </summary>
        SearchEngineOptimization,

        /// <summary>
        /// Terms and Conditions
        /// </summary>
        TermsAndConditions,

        /// <summary>
        /// LinkedIn Integration
        /// </summary>
        LinkedInIntegration,

        /// <summary>
        /// EEO
        /// </summary>
        EEO,

        /// <summary>
        /// ProspectRecommendation
        /// </summary>
        ProspectRecommendation,

        /// <summary>
        /// Relevance Search
        /// </summary>
        RelevanceSearch,

        /// <summary>
        /// Activity audience
        /// </summary>
        ActivityAudience,

        /// <summary>
        /// Apply with LinkedIn
        /// </summary>
        ApplyWithLinkedIn,

        /// <summary>
        /// Inclusive Hiring
        /// </summary>
        InclusiveHiring,

        /// <summary>
        /// Broadbean setup
        /// </summary>
        BroadbeanSetup,

        /// <summary>
        /// Job creation wizard
        /// </summary>
        JobCreationWizard,

        /// <summary>
        /// Feature enables user to track application and candidate level source. 
        /// </summary>
        TalentSourceTracking,

        /// <summary>
        /// Feature enables user to tag applicant as silvermedalist
        /// </summary>
        SilverMedalist,

        /// <summary>
        /// Feature enables service account
        /// </summary>
        ServiceAccount,
    }
}
