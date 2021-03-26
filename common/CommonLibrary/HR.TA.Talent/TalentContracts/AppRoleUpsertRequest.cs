//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.Web.Contracts;

    /// <summary>
    /// Contract class representing a request for upserting the list of user roles for a given user
    /// </summary>
    [DataContract]
    public class AppRoleUpsertRequest
    {
        /// <summary>
        /// Object Id of the user
        /// </summary>
        [DataMember(Name = "userObjectId", IsRequired = false, EmitDefaultValue = false)]
        public string UserObjectId { get; set; }

        /// <summary>
        /// User details
        /// </summary>
        [DataMember(Name = "user", IsRequired = false, EmitDefaultValue = false)]
        public Person User { get; set; }

        /// <summary>
        /// List of user roles to upsert
        /// </summary>
        [DataMember(Name = "userRoles", IsRequired = true, EmitDefaultValue = false)]
        public IList<TalentApplicationRole> UserRoles { get; set; }
    }
}
