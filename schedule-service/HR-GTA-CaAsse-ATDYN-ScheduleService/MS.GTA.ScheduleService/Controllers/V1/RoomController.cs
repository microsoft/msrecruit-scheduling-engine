// <copyright file="RoomController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// Interface with Microsoft Graph API to fetch Microsoft Buildings and Rooms required to specifiy the location of an interview.
    /// </summary>
    [Route("v1/rooms")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoomController : HCMWebApiAuthenticatedController
    {
        /// <summary>
        /// Gets or sets room resource
        /// </summary>
        private readonly IRoomResourceProvider roomResourceProvider;

        /// <summary>
        /// The instance for <see cref="ILogger{RoomController}"/>.
        /// </summary>
        private readonly ILogger<RoomController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="roomResourceProvider">The room resource provider.</param>
        /// <param name="logger">The instance for <see cref="ILogger{RoomController}"/>.</param>
        public RoomController(
            IHttpContextAccessor httpContextAccessor,
            IRoomResourceProvider roomResourceProvider,
            ILogger<RoomController> logger)
                        : base(httpContextAccessor)
        {
            this.roomResourceProvider = roomResourceProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches the list of Microsoft Buildings aka RoomLists from Microsoft Graph.
        /// </summary>
        /// <returns>An instance of <see cref="Task{T}" /> where <c>T</c> being <see cref="IList{Room}" />.</returns>
        [HttpGet]
        [Route("buildings")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(GetRoomsUnAuthorizedOrForbiddenException), 403)]
        [ProducesResponseType(typeof(SchedulerGetRoomsException), 500)]
        [ProducesResponseType(typeof(AggregateException), 500)]
        public async Task<List<Room>> GetRoomLists()
        {
            List<Room> rooms;
            this.logger.LogInformation($"Started {nameof(this.GetRoomLists)} method in {nameof(RoomController)}.");
            rooms = await this.roomResourceProvider.GetRoomLists();
            this.logger.LogInformation($"Finished {nameof(this.GetRoomLists)} method in {nameof(RoomController)}.");
            return rooms;
        }

        /// <summary>
        /// Fetches the list of conference rooms in a given Microsoft building from Microsoft Graph.
        /// </summary>
        /// <param name="buildingName">Identifier of the Building whose rooms list has to be fetched.</param>
        /// <returns>An instance of <see cref="Task{T}" /> where <c>T</c> being <see cref="IList{Room}" />.</returns>
        [HttpGet]
        [Route("{buildingName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(GetRoomsUnAuthorizedOrForbiddenException), 403)]
        [ProducesResponseType(typeof(SchedulerGetRoomsException), 500)]
        [ProducesResponseType(typeof(AggregateException), 500)]
        public async Task<List<Room>> GetRooms([FromRoute]string buildingName)
        {
            List<Room> rooms;
            this.logger.LogInformation($"Started {nameof(this.GetRooms)} method in {nameof(RoomController)}.");
            if (string.IsNullOrWhiteSpace(buildingName))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid room name").EnsureLogged(this.logger);
            }

            rooms = await this.roomResourceProvider.GetRooms(buildingName);
            this.logger.LogInformation($"Finished {nameof(this.GetRooms)} method in {nameof(RoomController)}.");
            return rooms;
        }
    }
}