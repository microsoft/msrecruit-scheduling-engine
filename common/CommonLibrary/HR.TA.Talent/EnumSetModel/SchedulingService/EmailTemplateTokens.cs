//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.EnumSetModel.SchedulingService
{
    using System.Runtime.Serialization;

    //
    // Summary:
    //     Email Template Tokens
    [DataContract]
    public enum EmailTemplateTokens
    {
        //
        // Summary:
        //     Job Title
        Job_Title = 0,
        //
        // Summary:
        //     Candidate Name,
        Candidate_Name = 1,
        //
        // Summary:
        //     Candidate First Name
        Candidate_FirstName = 2,
        //
        // Summary:
        //     Job Title
        Company_Name = 3,
        //
        // Summary:
        //     Requester Name - whoever is sending
        Requester_Name = 4,
        //
        // Summary:
        //     Requester email - whoever is sending email
        Requester_Email = 5,
        //
        // Summary:
        //     Requester Role
        Requester_Role = 6,
        //
        // Summary:
        //     Hiring Manager Name
        HiringManager_Name = 7,
        //
        // Summary:
        //     Hiring Manager email
        HiringManager_Email = 8,
        //
        // Summary:
        //     Interview Date. First Interview Date.
        Interview_Date = 9,
        //
        // Summary:
        //     Recruiter Name- This will be first recruiter in list
        Recruiter_Name = 10,
        //
        // Summary:
        //     Recruiter Email- This will be first recruiter email in list
        Recruiter_Email = 11,
        //
        // Summary:
        //     Interview Details Table - No Need to fill at client side
        Interview_Details_Table = 12,
        //
        // Summary:
        //     Button Href => Only default template will have it. No need to fill at client
        //     side.
        Call_To_Action_Link = 13,
        //
        // Summary:
        //     First Interviewer Name
        First_Interviewer_Name = 14,
        //
        // Summary:
        //     First Interview Time
        First_Interview_Time = 15,
        //
        // Summary:
        //     First Interviewer Job Title
        First_Interviewer_Job_Title = 16,
        //
        // Summary:
        //     Scheduler Email
        Scheduler_Email = 17,
        //
        // Summary:
        //     Scheduler Phone Number
        Scheduler_Phone_Number = 18,
        //
        // Summary:
        //     Skype Link
        Skype_Link = 19,
        //
        // Summary:
        //     Job Id
        Job_Id = 20,
        //
        // Summary:
        //     External Job Id
        External_Job_Id = 21,
        //
        // Summary:
        //     Interviewer first name
        Interviewer_FirstName = 22,
        //
        // Summary:
        //     First name of interview scheduler
        Scheduler_First_Name = 23,
        //
        // Summary:
        //     Calendar event response status
        ResponseStatus = 24,
        //
        // Summary:
        //     Offer Expiry Date
        OfferExpiryDate = 25,
        //
        // Summary:
        //     Offer Package URL
        OfferPackageURL = 26,
        //
        // Summary:
        //     Offer Approver Name
        Offer_Approver_Name = 27,
        //
        // Summary:
        //     Offer Approver Email
        Offer_Approver_Email = 28,
        //
        // Summary:
        //     Offer Rejecter Name
        Offer_Rejecter_Name = 29,
        //
        // Summary:
        //     Offer Rejecter Email
        Offer_Rejecter_Email = 30,
        //
        // Summary:
        //     User Auth Provider
        Identity_Provider = 31,
        //
        // Summary:
        //     User Auth Provider
        Identity_Provider_User_Name = 32,
        //
        // Summary:
        //     Job Approver Name
        Job_Approver_Name = 33,
        //
        // Summary:
        //     Job Approver Email
        Job_Approver_Email = 34,
        //
        // Summary:
        //     Job Requester Name
        Job_Requester_Name = 35,
        //
        // Summary:
        //     Job Requester Email
        Job_Requester_Email = 36
    }
}
