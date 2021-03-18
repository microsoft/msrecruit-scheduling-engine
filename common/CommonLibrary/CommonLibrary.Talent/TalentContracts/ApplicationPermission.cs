//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for application level permission 
    /// It defines restricted permission on action which is in application scope but outside activity. 
    /// </summary>
    [DataContract]
    public enum ApplicationPermission
    {
        /// <summary>
        /// Read Note
        /// </summary>
        ReadNote,

        /// <summary>
        /// Create Note
        /// </summary>
        CreateNote,

        /// <summary>
        /// Create Offer
        /// </summary>
        CreateOffer,

        /// <summary>
        /// Read application
        /// </summary>
        ReadApplication,

        /// <summary>
        /// Update application
        /// </summary>
        UpdateApplication,

        /// <summary>
        /// Delete application
        /// </summary>
        DeleteApplication,

        /// <summary>
        /// Reject Applicant
        /// </summary>
        RejectApplicant,

        /// <summary>
        /// View all activities, enum is reader to give read access to activities
        /// </summary>
        ViewAllActivities,

        /// <summary>
        /// Update all activites, enum for all admin to give update access to activities
        /// </summary>
        UpdateAllActivities,
    }
}
