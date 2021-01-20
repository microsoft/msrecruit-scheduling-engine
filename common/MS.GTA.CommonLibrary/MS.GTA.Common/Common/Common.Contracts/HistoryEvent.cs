﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HistoryEvent.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>The history event.</summary>
    [DataContract]
    public class HistoryEvent
    {
        /// <summary>Gets or sets the logs.</summary>
        [DataMember(Name = "logs")]
        public List<string> Logs { get; set; } = new List<string>();

        /// <summary>Gets or sets the raid.</summary>
        [DataMember(Name = "raid")]
        public string Raid { get; set; }

        /// <summary>Gets or sets the date.</summary>
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}