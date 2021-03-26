//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TA.CommonLibrary.Common.DocumentDB.Contracts;
using TA.CommonLibrary.Common.TalentEntities.Common;
using TA.CommonLibrary.TalentEntities.Enum;
using TA.CommonLibrary.Common.TalentEntities.Enum.Common;
using TA.CommonLibrary.Talent.FalconEntities.Attract;

namespace TA.CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract
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
