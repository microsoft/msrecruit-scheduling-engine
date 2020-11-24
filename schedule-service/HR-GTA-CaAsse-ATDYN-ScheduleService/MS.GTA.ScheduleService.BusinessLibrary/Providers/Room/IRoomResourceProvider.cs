// <copyright file="IRoomResourceProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using MS.GTA.ScheduleService.Contracts.V1;

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    /// <summary>
    /// Interface to define room resource related operations
    /// </summary>
    public interface IRoomResourceProvider
    {
        /// <summary>
        /// Gets room list collection
        /// </summary>
        /// <returns>A list of rooms</returns>
        Task<List<Room>> GetRoomLists();

        /// <summary>
        /// Gets room collection
        /// </summary>
        /// <param name="buildingName">Building name</param>
        /// <returns>A list of rooms</returns>
        Task<List<Room>> GetRooms(string buildingName);
    }
}
