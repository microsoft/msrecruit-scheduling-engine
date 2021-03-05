//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The job data contract.
    /// </summary>
    [DataContract]
    public class JobOpeningTemplateParticipant : AADUser
    {
        /// <summary>Gets or sets user object id.</summary>
        [DataMember(Name = "userObjectId", IsRequired = false)]
        public string UserObjectId { get; set; }

        /// <summary>Gets or sets group object id.</summary>
        [DataMember(Name = "groupObjectId", IsRequired = false)]
        public string GroupObjectId { get; set; }

        /// <summary>Gets or sets tenant object id.</summary>
        [DataMember(Name = "tenantObjectId", IsRequired = false)]
        public string TenantObjectId { get; set; }

        /// <summary>Gets or sets default flag.</summary>
        [DataMember(Name = "isDefault", IsRequired = false)]
        public bool IsDefault { get; set; }

        /// <summary>Gets or sets default flag.</summary>
        [DataMember(Name = "canEdit", IsRequired = false)]
        public bool CanEdit { get; set; }
    }
}
