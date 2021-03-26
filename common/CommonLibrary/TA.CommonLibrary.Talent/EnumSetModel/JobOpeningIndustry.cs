//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening industries.
    /// </summary>
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobOpeningIndustry
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
