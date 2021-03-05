//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job grade types.
    /// </summary>
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobOpeningJobGrade
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
