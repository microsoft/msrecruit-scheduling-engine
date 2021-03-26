//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Job level permission 
    /// It defines restricted permission on action which is in job scope but outside application. 
    /// </summary>
    [DataContract]
    public enum JobPermission
    {
        /// <summary>
        /// Update Job Details
        /// </summary>
        UpdateJobDetails,

        /// <summary>
        /// Update Job Process
        /// </summary>
        UpdateJobProcess,

        /// <summary>
        /// Activate Job
        /// </summary>
        ActivateJob,

        /// <summary>
        /// Create Job Approval
        /// </summary>
        CreateJobApproval,

        /// <summary>
        /// Create Job Posting
        /// </summary>
        CreateJobPosting,

        /// <summary>
        /// Close Job
        /// </summary>
        CloseJob,

        /// <summary>
        /// Create Applicant
        /// </summary>
        CreateApplicant,

        /// <summary>
        /// Remove job
        /// </summary>
        DeleteJob,

        /// <summary>
        /// Create hiring team
        /// </summary>
        CreateHiringTeam
    }
}
