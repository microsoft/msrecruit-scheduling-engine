//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// UserGroup properties
    /// </summary>
    [DataContract]
    public class UserGroup
    {
        /// <summary>
        /// Gets or sets the free busy id used to correlate responses
        /// </summary>
        [DataMember(Name = "freeBusyTimeId", IsRequired = false, EmitDefaultValue = false)]
        public string FreeBusyTimeId { get; set; }

        /// <summary>
        /// Gets or sets the list of users
        /// </summary>
        [DataMember(Name = "users")]
        public List<GraphPerson> Users { get; set; }
    }
}
