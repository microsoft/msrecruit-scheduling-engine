//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
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
        /// Candidate Name,
        /// </summary>
        Candidate_Name = 1,

        /// <summary>
        /// Candidate First Name
        /// </summary>
        Candidate_FirstName = 2,

        /// <summary>
        /// Job Title
        /// </summary>
        Company_Name = 3,

        /// <summary>
        /// Requester Name - whoever is sending
        /// </summary>
        Requester_Name = 4,

        /// <summary>
        /// Requester email - whoever is sending email
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
        /// Hiring Manager email
        /// </summary>
        HiringManager_Email = 8,

        /// <summary>
        /// Interview Date. First Interview Date.
        /// </summary>
        Interview_Date = 9,

        /// <summary>
        /// Recruiter Name- This will be first recruiter in list
        /// </summary>
        Recruiter_Name = 10,

        /// <summary>
        /// Recruiter Email- This will be first recruiter email in list
        /// </summary>
        Recruiter_Email = 11,

        /// <summary>
        /// Interview Details Table - No Need to fill at client side
        /// </summary>
        Interview_Details_Table = 12,

        /// <summary>
        /// Button Href => Only default template will have it. No need to fill at client side.
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
        /// Interviewer first name
        /// </summary>
        Interviewer_FirstName = 22,

        /// <summary>
        /// First name of interview scheduler
        /// </summary>
        Scheduler_First_Name = 23,

        /// <summary>
        /// Calendar event response status
        /// </summary>
        ResponseStatus = 24,

        /// <summary>
        /// Offer Expiry Date
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
        /// Offer Rejecter Name
        /// </summary>
        Offer_Rejecter_Name = 29,

        /// <summary>
        /// Offer Rejecter Email
        /// </summary>
        Offer_Rejecter_Email = 30,

        /// <summary>
        /// User Auth Provider
        /// </summary>
        Identity_Provider = 31,

        /// <summary>
        /// User Auth Provider
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
        Job_Requester_Email = 36,

        /// <summary>
        /// Link to Profile
        /// </summary>
        Profile_Link = 37,

        /// <summary>
        /// Delegatin From name
        /// </summary>
        Delegation_From_Name,

        /// <summary>
        /// Delegatin To name
        /// </summary>
        Delegation_To_Name,

        /// <summary>
        /// Delegation from date
        /// </summary>
        Delegation_From_Date,

        /// <summary>
        /// Delegation to date.
        /// </summary>
        Delegation_To_Date
    }
}
