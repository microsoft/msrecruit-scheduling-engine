//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ScheduleService.Contracts.V1
{
    using CommonLibrary.Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The primary entity representing a scheduling event
    /// </summary>
    [DataContract]
    public class ScheduleEvent
    {
        /// <summary>
        /// Gets or sets the unique id of the scheduling event
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the interviewee email address
        /// </summary>
        [DataMember(Name = "candidate")]
        public GraphPerson Candidate { get; set; }

        /// <summary>
        /// Gets or sets the list of dates for candidate availability
        /// </summary>
        [DataMember(Name = "dates")]
        public List<string> Dates { get; set; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false, EmitDefaultValue = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets job title
        /// </summary>
        [DataMember(Name = "jobTitle", IsRequired = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets user permissions
        /// </summary>
        [DataMember(Name = "userPermissions", IsRequired = false, EmitDefaultValue = false)]
        public List<UserPermission> UserPermissions { get; set; }

        /// <summary>
        /// Gets or sets the path and query portion of the Url to include in the body of the mail.  Expected to begin with a / (slash).
        /// </summary>
        [DataMember(Name = "deepLinkUrl", IsRequired = false, EmitDefaultValue = false)]
        public string DeepLinkUrl { get; set; }

        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        [DataMember(Name = "timezone", IsRequired = false, EmitDefaultValue = false)]
        public Timezone Timezone { get; set; }
    }
}
