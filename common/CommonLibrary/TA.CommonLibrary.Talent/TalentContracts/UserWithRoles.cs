//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.Web.Contracts;

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
