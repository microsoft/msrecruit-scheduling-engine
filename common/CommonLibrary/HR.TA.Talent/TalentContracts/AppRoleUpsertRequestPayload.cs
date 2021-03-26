//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class representing a list of role upserts to perform for numerous users
    /// </summary>
    [DataContract]
    public class AppRoleUpsertRequestPayload
    {
        /// <summary>
        /// List of user role upsert requests
        /// </summary>
        [DataMember(Name = "roleUpsertRequests", IsRequired = true, EmitDefaultValue = false)]
        public IList<AppRoleUpsertRequest> RoleUpsertRequests { get; set; }
    }
}
