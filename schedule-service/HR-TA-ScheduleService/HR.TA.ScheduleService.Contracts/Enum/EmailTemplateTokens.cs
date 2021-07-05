//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.EnumSetModel.SchedulingService
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Email Template Tokens
    /// </summary>
    [DataContract]
    public enum EmailTemplateTokens
    {
        /// <summary>
        /// Job Title
        /// </summary>
        Job_Title = 0,
        
        /// <summary>
        /// candidate Name
        /// </summary>
        Candidate_Name = 1,

        /// <summary>
        /// Candidate First Name
        /// </summary>
        Candidate_FirstName = 2,
        
        /// <summary>
        /// Company Name
        /// </summary>
        Company_Name = 3,
        
        /// <summary>
        /// Requester Name
        /// </summary>
        Requester_Name = 4,
        
        /// <summary>
        /// Requester Email
        /// </summary>
        Requester_Email = 5,
        
        /// <summary>
        /// Requester Role
        /// </summary>
        Requester_Role = 6,

        /// <summary>
        /// Hiring Manager Name
        /// </summary>
        HiringManager_Name = 7,

        /// <summary>
        /// Hiring Manager Email
        /// </summary>
        HiringManager_Email = 8,

        /// <summary>
        /// Interview Date
        /// </summary>
        Interview_Date = 9,

        /// <summary>
        /// Recruiter Name
        /// </summary>
        Recruiter_Name = 10,

        /// <summary>
        /// Recruiter Email
        /// </summary>
        Recruiter_Email = 11,

        /// <summary>
        /// Interview Details Table
        /// </summary>
        Interview_Details_Table = 12,
        /// <summary>
        /// Call To Action Link
        /// </summary>
        Call_To_Action_Link = 13,
        /// <summary>
        /// First Interviewer Name
        /// </summary>
        First_Interviewer_Name = 14,
        /// <summary>
        /// First Interview Time
        /// </summary>
        First_Interview_Time = 15,
        /// <summary>
        /// First Interviewer Job Title
        /// </summary>
        First_Interviewer_Job_Title = 16,
        /// <summary>
        /// Scheduler Email
        /// </summary>
        Scheduler_Email = 17,
        /// <summary>
        /// Scheduler Phone Number
        /// </summary>
        Scheduler_Phone_Number = 18,
        /// <summary>
        /// Skype Link
        /// </summary>
        Skype_Link = 19,
        /// <summary>
        /// Job Id
        /// </summary>
        Job_Id = 20,
        /// <summary>
        /// External Job Id
        /// </summary>
        External_Job_Id = 21,
        /// <summary>
        /// Interviewer First Name
        /// </summary>
        Interviewer_FirstName = 22,
        /// <summary>
        /// Scheduler First Name
        /// </summary>
        Scheduler_First_Name = 23,
        /// <summary>
        /// Response Status
        /// </summary>
        ResponseStatus = 24,
        /// <summary>
        /// Offer Expiry date
        /// </summary>
        OfferExpiryDate = 25,
        /// <summary>
        /// Offer Package URL
        /// </summary>
        OfferPackageURL = 26,
        /// <summary>
        /// Offer Approver Name
        /// </summary>
        Offer_Approver_Name = 27,
        /// <summary>
        /// Offer Approver Email
        /// </summary>
        Offer_Approver_Email = 28,
        /// <summary>
        /// Offer Rejecter name
        /// </summary>
        Offer_Rejecter_Name = 29,
        /// <summary>
        /// Offer rejecter Email
        /// </summary>
        Offer_Rejecter_Email = 30,
        /// <summary>
        /// Identity provider
        /// </summary>
        Identity_Provider = 31,
        /// <summary>
        /// Identity Provider User Name
        /// </summary>
        Identity_Provider_User_Name = 32,
        /// <summary>
        /// Job Approver Name
        /// </summary>
        Job_Approver_Name = 33,
        /// <summary>
        /// Job Approver Email
        /// </summary>
        Job_Approver_Email = 34,
        /// <summary>
        /// Job Requester Name
        /// </summary>
        Job_Requester_Name = 35,
        /// <summary>
        /// Job Requester Email
        /// </summary>
        Job_Requester_Email = 36
    }
}
