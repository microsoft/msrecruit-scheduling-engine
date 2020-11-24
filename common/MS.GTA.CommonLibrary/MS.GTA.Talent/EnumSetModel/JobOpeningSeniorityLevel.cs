// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningSeniorityLevel.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening seniority levels.
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningSeniorityLevel
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0,

        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "internship")]
        Internship = 1,

        /// <summary>
        /// Entry level.
        /// </summary>
        [EnumMember(Value = "entryLevel")]
        EntryLevel = 2,

        /// <summary>
        /// Associate.
        /// </summary>
        [EnumMember(Value = "associate")]
        Associate = 3,

        /// <summary>
        /// Mid senior level.
        /// </summary>
        [EnumMember(Value = "midSeniorLevel")]
        MidSeniorLevel = 4,

        /// <summary>
        /// Director.
        /// </summary>
        [EnumMember(Value = "director")]
        Director = 5,

        /// <summary>
        /// Executive.
        /// </summary>
        [EnumMember(Value = "executive")]
        Executive = 6
    }
}
