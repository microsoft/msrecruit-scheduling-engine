//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for app level permission 
    /// It defines restricted permission on action outside job. 
    /// </summary>
    [DataContract]
    public enum AppPermission
    {
        /// <summary>
        /// Read talent pool 
        /// </summary>
        ReadTalentPool,

        /// <summary>
        /// Create Talent Pool
        /// </summary>
        CreateTalentPool,

        /// <summary>
        /// Read template
        /// </summary>
        ReadTemplate,

        /// <summary>
        /// Create template
        /// </summary>
        CreateTemplate,

        /// <summary>
        /// Create job
        /// </summary>
        CreateJob,

        /// <summary>
        /// Read email template
        /// /// </summary>
        ReadEmailTemplate,

        /// <summary>
        /// Create email template
        /// </summary>
        CreateEmailTemplate,

        /// <summary>
        /// Access admin center
        /// </summary>
        AccessAdminCenter,

        /// <summary>
        /// View all jobs, enum for admin/reader to give access to view all jobs
        /// </summary>
        ViewAllJobs,

        /// <summary>
        /// View all jobs, enum for admin/reader to give access to view all applications
        /// </summary>
        ViewAllJobApplications,

        /// <summary>
        /// View all jobs, enum for admin/reader to give access to view all talent pools
        /// </summary>
        ViewAllTalentPools,

        /// <summary>
        /// View all templates, enum for admin/reader to give access to view all templates
        /// </summary>
        ViewAllTemplates,

        /// <summary>
        /// Update all candidate, enum for admin/reader to give access to update all candidates
        /// </summary>
        UpdateAllCandidates,

        /// <summary>
        /// View all analytics, enum for admin/reader to give access to view all analytics
        /// </summary>
        ViewAnalytics,
    }
}
