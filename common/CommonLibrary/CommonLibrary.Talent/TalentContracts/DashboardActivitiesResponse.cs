//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class DashboardActivitiesResponse
    {
        /// <summary>
        /// Gets or sets dashboard activities for user
        /// </summary>
        [DataMember(Name = "dashboardActivities", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<DashboardActivity> DashboardActivities { get; set; }

        /// <summary>
        /// Gets or sets continuation token for skip
        /// </summary>
        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }
    }
}
