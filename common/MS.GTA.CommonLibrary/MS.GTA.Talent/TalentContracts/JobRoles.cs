//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Contract class with the list of job roles for the user.
    /// </summary>
    [DataContract]
    public class JobRoles
    {
        /// <summary>
        /// Gets or sets the list of job participant roles
        /// </summary>
        [DataMember(Name = "jobParticipantRoles", IsRequired = true, EmitDefaultValue = false)]
        public IList<JobParticipantRole> JobParticipantRoles { get; set; }
    }
}
