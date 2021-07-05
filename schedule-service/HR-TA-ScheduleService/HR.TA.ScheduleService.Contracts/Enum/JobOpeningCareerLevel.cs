//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening career levels.
    /// </summary>
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobOpeningCareerLevel
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0,
    }
}
