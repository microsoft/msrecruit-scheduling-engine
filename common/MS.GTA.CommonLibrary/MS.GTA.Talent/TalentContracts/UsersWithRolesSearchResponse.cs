//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;

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
