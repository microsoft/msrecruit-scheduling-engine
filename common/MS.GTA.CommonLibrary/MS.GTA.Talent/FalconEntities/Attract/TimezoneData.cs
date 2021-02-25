//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Talent.FalconEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    /// <summary>
    /// Timezone Data for jobapplication
    /// </summary>
    [DataContract]
    public class TimezoneData : DocDbEntity
    {
        /// <summary>
        /// Gets or sets jobapplicationid
        /// </summary>
        [DataMember(Name = "JobApplicationId", EmitDefaultValue = true, IsRequired = true)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets TimezoneName
        /// </summary>
        [DataMember(Name = "TimezoneName", IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneName { get; set; }

        /// <summary>
        /// Gets or sets UTCOffset
        /// </summary>
        [DataMember(Name = "UTCOffset", IsRequired = false, EmitDefaultValue = false)]
        public int UTCOffset { get; set; }

        /// <summary>
        /// Gets or sets UTCOffsetInHours
        /// </summary>
        [DataMember(Name = "UTCOffsetInHours", IsRequired = false, EmitDefaultValue = false)]
        public string UTCOffsetInHours { get; set; }

        /// <summary>
        /// Gets or sets TimezoneAbbr
        /// </summary>
        [DataMember(Name = "TimezoneAbbr", IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneAbbr { get; set; }
    }
}
