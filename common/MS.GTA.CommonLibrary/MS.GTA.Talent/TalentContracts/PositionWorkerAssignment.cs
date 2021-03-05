//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TalentEngagementService.Data;

    /// <summary>
    /// The Department.
    /// </summary>
    [DataContract]
    public class PositionWorkerAssignment
    {
        /// <summary>Gets or sets jobPosition id</summary>
        [DataMember(Name = "jobPositionId", IsRequired = false)]
        public string JobPositionId { get; set; }

        /// <summary>Gets or sets map id</summary>
        [DataMember(Name = "mapId", IsRequired = false)]
        public string MapId { get; set; }

        /// <summary>Gets or sets number</summary>
        [DataMember(Name = "number", IsRequired = false)]
        public string Number { get; set; }

        /// <summary>Gets or sets worker id</summary>
        [DataMember(Name = "workerId", IsRequired = false)]
        public string WorkerId { get; set; }
    }
}
