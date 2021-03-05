//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.DocumentDB.Contracts;
using Common.TalentEntities.Common;
using TalentEntities.Enum;
using Common.TalentEntities.Enum.Common;
using Talent.FalconEntities.Attract;

namespace Common.Provisioning.Entities.FalconEntities.Attract
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

        /// <summary>
        /// Gets or sets UTCOffsetInMinutes
        /// </summary>
        [DataMember(Name = "UTCOffsetInMinutes", IsRequired = false, EmitDefaultValue = false)]
        public string UTCOffsetInMinutes { get; set; }

    }
}
