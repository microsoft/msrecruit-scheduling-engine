//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.Web.Contracts;

    /// <summary>
    /// Contract class with the permissions for a user.
    /// </summary>
    [DataContract]
    public class AppRoles
    {
        /// <summary>
        /// Gets or sets the list of app level user permissions.
        /// </summary>
        [DataMember(Name = "appRoles", IsRequired = true, EmitDefaultValue = false)]
        public IList<TalentApplicationRole> UserRoles { get; set; }
    }
}
