﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Integration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The integration contract.
    /// </summary>
    [DataContract]
    public class Integration
    {        
        /// <summary>Gets or sets jobs.</summary>
        [DataMember(Name = "jobs", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Job> Jobs { get; set; }

        /// <summary>Gets or sets sync time.</summary>
        [DataMember(Name = "syncTimeInTicks", IsRequired = false, EmitDefaultValue = false)]
        public long SyncTimeinTicks { get; set; }
    }
}
