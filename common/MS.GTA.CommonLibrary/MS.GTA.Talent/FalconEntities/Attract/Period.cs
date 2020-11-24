//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MS.GTA.Common.DocumentDB.Contracts;
using MS.GTA.Common.TalentEntities.Common;
using MS.GTA.TalentEntities.Enum;
using MS.GTA.Common.TalentEntities.Enum.Common;
using MS.GTA.Talent.FalconEntities.Attract;

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    /// <summary>
    /// Entity to store the delegation request period
    /// </summary>
    [DataContract]
    public class Period 
    {
        /// <summary>
        /// Get or set delegator from date
        /// </summary>
        [DataMember(Name = "FromDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Get or set delegator to date
        /// </summary>
        [DataMember(Name = "ToDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ToDate { get; set; }

    }
}
