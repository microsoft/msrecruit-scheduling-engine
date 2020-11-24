//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentDocument.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Documents;

    [DataContract]
    /// <summary>The environment document used when storing environment provisioning status information in the cosmos DB.</summary>
    public class EnvironmentDocument : Document
    {
        /// <summary>Gets or sets the environment status.</summary>
        [DataMember(Name = "environmentStatus")]
        public EnvironmentStatus EnvironmentStatus { get; set; } = new EnvironmentStatus();

        /// <summary>Gets or sets the environment history.</summary>
        [DataMember(Name = "environmentHistory")]
        public List<HistoryEvent> EnvironmentHistory { get; set; } = new List<HistoryEvent>();

        /// <summary>Gets or sets the created date.</summary>
        [DataMember(Name = "createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
