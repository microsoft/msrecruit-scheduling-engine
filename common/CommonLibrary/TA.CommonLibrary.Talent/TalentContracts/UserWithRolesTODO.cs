//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    ////using TA.CommonLibrary.Common.Web.Contracts;

    /// <summary>
    /// User with assigned roles.
    /// /// </summary>
    [DataContract]
    public class UserWithRolesTODO : AADUser
    {
        /// <summary>
        /// Gets or sets the collection of roles
        /// </summary>
        [DataMember(Name = "roles", IsRequired = false, EmitDefaultValue = false)]
        public string Roles { get; set; }
    }
}
