//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening incentive plan.
    /// </summary>
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobOpeningIncentivePlan
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
