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
