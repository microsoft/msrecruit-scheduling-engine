//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The talent pool contract.
    /// </summary>
    [DataContract]
    public class TalentEEOAccessTrail
    {
        [DataMember(Name = "toDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ToDate { get; set; }

        [DataMember(Name = "fromDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? FromDate { get; set; }

        [DataMember(Name = "requestedOn", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? RequestedOn { get; set; }

        [DataMember(Name = "worker", EmitDefaultValue = false, IsRequired = false)]
        public Person Worker { get; set; }
    }
}
