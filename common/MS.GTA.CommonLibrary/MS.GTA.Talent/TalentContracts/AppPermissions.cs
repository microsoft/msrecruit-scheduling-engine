﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class with the permissions for a user.
    /// </summary>
    [DataContract]
    public class AppPermissions
    {
        /// <summary>
        /// Gets or sets the list of app level user permissions.
        /// </summary>
        [DataMember(Name = "userPermissions", IsRequired = false, EmitDefaultValue = false)]
        public IList<AppPermission> UserPermissions { get; set; }
    }
}
