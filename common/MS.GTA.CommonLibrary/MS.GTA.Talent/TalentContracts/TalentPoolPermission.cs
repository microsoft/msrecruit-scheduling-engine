//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentPoolPermission.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for talent pool permission
    /// </summary>
    [DataContract]
    public enum TalentPoolPermission
    {
        /// <summary>
        /// Read TalentPool
        /// </summary>
        ReadTalentPool,

        /// <summary>
        /// Update TalentPool
        /// </summary>
        UpdateTalentPool,

        /// <summary>
        /// Delete TalentPool
        /// </summary>
        DeleteTalentPool,

        /// <summary>
        /// Add Candidate
        /// </summary>
        AddCandidate,

        /// <summary>
        /// Delete Candidate
        /// </summary>
        DeleteCandidate,
    }
}
