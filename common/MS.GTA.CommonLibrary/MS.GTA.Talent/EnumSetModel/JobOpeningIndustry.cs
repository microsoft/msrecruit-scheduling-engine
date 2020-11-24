// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningIndustry.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening industries.
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningIndustry
    {
        /// <summary>
        /// Default value placeholder.
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }
}
