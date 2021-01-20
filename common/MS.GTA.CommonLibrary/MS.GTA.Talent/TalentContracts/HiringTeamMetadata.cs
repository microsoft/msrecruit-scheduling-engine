//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HiringTeamMetadata.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// HiringTeam Metadata Request
    /// </summary>
    [DataContract]
    public class HiringTeamMetadata
    {
        /// <summary>
        /// Gets or sets Total
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets Hiring Team list
        /// </summary>
        [DataMember(Name = "hiringTeam", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<HiringTeamMember> HiringTeam { get; set; }

        /// <summary>
        /// Gets or sets Delegates list
        /// </summary>
        [DataMember(Name = "delegates", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Delegate> Delegates { get; set; }
    }
}
