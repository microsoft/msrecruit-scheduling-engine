//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentPool.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The talent pool contract.
    /// </summary>
    [DataContract]
    public class TalentPool
    {
        [DataMember(Name = "poolId", EmitDefaultValue = false, IsRequired = false)]
        public string PoolId { get; set; }

        [DataMember(Name = "poolName", EmitDefaultValue = false, IsRequired = false)]
        public string PoolName { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "candidates", EmitDefaultValue = false, IsRequired = false)]
        public IList<Applicant> Candidates { get; set; }

        [DataMember(Name = "candidateCount", EmitDefaultValue = false, IsRequired = false)]
        public int? CandidateCount { get; set; }

        [DataMember(Name = "contributors", EmitDefaultValue = false, IsRequired = false)]
        public IList<TalentPoolParticipant> Contributors { get; set; }

        [DataMember(Name = "lastModified", EmitDefaultValue = false, IsRequired = false)]
        public DateTime LastModified { get; set; }

        [DataMember(Name = "source", IsRequired = false)]
        public TalentPoolSource Source { get; set; }

        [DataMember(Name = "externalId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the list of permissions to the talent pool for user.
        /// </summary>
        [DataMember(Name = "userPermissions", IsRequired = false, EmitDefaultValue = false)]
        public IList<TalentPoolPermission> UserPermissions { get; set; }
    }
}
