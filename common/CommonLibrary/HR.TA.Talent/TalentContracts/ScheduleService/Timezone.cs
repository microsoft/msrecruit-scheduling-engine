//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Timezone contract
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class Timezone
    {
        /// <summary>
        /// Gets or sets the timezone name.
        /// </summary>
        [DataMember(Name = "timezoneName", IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneName { get; set; }

        /// <summary>
        /// Gets or sets the utc offset.
        /// </summary>
        [DataMember(Name = "utcOffset", IsRequired = false, EmitDefaultValue = false)]
        public int UTCOffset { get; set; }

        /// <summary>
        /// Gets or sets the utc offset in hours.
        /// </summary>
        [DataMember(Name = "utcOffsetHours", IsRequired = false, EmitDefaultValue = false)]
        public string UTCOffsetInHours { get; set; }

        /// <summary>
        /// Gets or sets the timezone abbreviation.
        /// </summary>
        [DataMember(Name = "timezoneAbbr", IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneAbbr { get; set; }
    }
}
