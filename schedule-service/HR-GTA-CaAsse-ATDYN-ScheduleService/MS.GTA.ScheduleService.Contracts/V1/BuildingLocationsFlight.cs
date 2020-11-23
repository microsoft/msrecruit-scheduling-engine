// <copyright file="BuildingLocationsFlight.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using MS.GTA.ServicePlatform.Flighting;

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
