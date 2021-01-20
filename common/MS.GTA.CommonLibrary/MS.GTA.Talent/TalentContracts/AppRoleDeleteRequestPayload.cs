//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AppRoleDeleteRequestPayload.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class representing a request to delete all roles for a list of users
    /// </summary>
    [DataContract]
    public class AppRoleDeleteRequestPayload
    {
        /// <summary>
        /// List of users to delete all roles for
        /// </summary>
        [DataMember(Name = "userObjectIds", IsRequired = true, EmitDefaultValue = false)]
        public IList<string> UserObjectIds { get; set; }
    }
}
