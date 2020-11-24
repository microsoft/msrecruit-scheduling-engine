//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationParticipantMetadata.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Job application stage data contract.
    /// </summary>
    [DataContract]
    public class JobApplicationParticipantMetadata
    {
        /// <summary>Gets or sets the list of allowed stages.</summary>
        [DataMember(Name = "stages")]
        public List<JobApplicationStage> Stages { get; set; }
    }
}
