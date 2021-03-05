//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Web.Contracts;

    /// <summary>
    /// User with assigned roles.
    /// /// </summary>
    [DataContract]
    public class UserWithRoles : AADUser
    {
        /// <summary>
        /// Gets or sets the collection of roles
        /// </summary>
        [DataMember(Name = "roles", IsRequired = false, EmitDefaultValue = false)]
        public IList<TalentApplicationRole> Roles { get; set; }
    }
}
