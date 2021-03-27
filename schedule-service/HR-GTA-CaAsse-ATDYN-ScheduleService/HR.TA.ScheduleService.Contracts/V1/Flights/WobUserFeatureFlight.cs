//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1.Flights
{
    using HR.TA.ServicePlatform.Flighting;

    /// <summary>
    /// The <see cref="WobUserFeatureFlight"/> class provides a switch to enable wob user feature.
    /// </summary>
    /// <seealso cref="SingletonFlight{WobUserFeatureFlight}" />
    public class WobUserFeatureFlight : SingletonFlight<WobUserFeatureFlight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WobUserFeatureFlight"/> class.
        /// </summary>
        public WobUserFeatureFlight()
            : base("IV_WOBUserFeature", "2020-08-13")
        {
        }
    }
}
