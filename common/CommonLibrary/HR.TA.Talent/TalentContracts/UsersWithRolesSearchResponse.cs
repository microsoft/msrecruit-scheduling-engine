//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.Contracts;

    /// <summary>
    /// Contract class representing a request for a list of users with roles
    /// </summary>
    [DataContract]
    public class UsersWithRolesSearchResponse
    {
        /// <summary>
        /// Gets or sets the collection of users
        /// </summary>
        [DataMember(Name = "users", IsRequired = false, EmitDefaultValue = false)]
        public IList<UserWithRoles> Users { get; set; }

        /// <summary>
        /// Gets or sets the total number of records
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int? Total { get; set; }
    }
}
