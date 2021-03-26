//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
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
