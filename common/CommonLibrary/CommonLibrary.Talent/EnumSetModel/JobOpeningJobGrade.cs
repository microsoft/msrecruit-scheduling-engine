//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job grade types.
    /// </summary>
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobOpeningJobGrade
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
