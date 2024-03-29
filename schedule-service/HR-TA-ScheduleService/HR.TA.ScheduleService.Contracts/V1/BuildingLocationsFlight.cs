//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using HR.TA.ServicePlatform.Flighting;

    /// <summary>
    /// Flighting for handling dashboard performance
    /// </summary>
    public class BuildingLocationsFlight : SingletonFlight<BuildingLocationsFlight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingLocationsFlight"/> class.
        /// </summary>
        public BuildingLocationsFlight()
            : base("EnableBuildingLocations", "2020-01-30")
        {
        }
    }
}
