// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningJobGrade.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job grade types.
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningJobGrade
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
