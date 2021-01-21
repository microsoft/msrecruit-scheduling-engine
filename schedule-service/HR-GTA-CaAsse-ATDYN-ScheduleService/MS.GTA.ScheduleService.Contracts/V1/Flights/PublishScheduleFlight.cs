// <copyright file="PublishScheduleFlight.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1.Flights
{
    using MS.GTA.ServicePlatform.Flighting;

    /// <summary>
    /// The <see cref="PublishScheduleFlight"/> class provides a switch to enable push message in connector queue for downstream system.
    /// </summary>
    /// <seealso cref="SingletonFlight{PublishScheduleFlight}" />
    public class PublishScheduleFlight : SingletonFlight<PublishScheduleFlight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishScheduleFlight"/> class.
        /// </summary>
        public PublishScheduleFlight()
            : base("EnablePublishIVSchedule", "2020-09-22")
        {
        }
    }
}
