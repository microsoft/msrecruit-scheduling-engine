// <copyright file="IOutlookProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The interface for the outlook service provider.
    /// </summary>
    public interface IOutlookProvider
    {
        /// <summary>
        /// Sends find free busy schedule request.
        /// </summary>
        /// <param name="findFreeBusyRequest">Request body.</param>
        /// <returns>FindFreeBusySchedule Response.</returns>
        Task<List<FindFreeBusyScheduleResponse>> SendPostFindFreeBusySchedule(FindFreeBusyScheduleRequest findFreeBusyRequest);

        /// <summary>
        /// Get calendar event request.
        /// </summary>
        /// <param name="userAccessToken">User Id token.</param>
        /// <param name="calendarEventId">calendar event id.</param>
        /// <param name="serviceAccountName">Service account email.</param>
        /// <returns>Calendar event that was retrieved</returns>
        Task<CalendarEvent> GetCalendarEvent(string userAccessToken, string calendarEventId, string serviceAccountName);

        /// <summary>
        /// Sends create event request.
        /// </summary>
        /// <param name="serviceAccountName">Service Account Name.</param>
        /// <param name="eventRequest">Request body.</param>
        /// <returns>Event that was created</returns>
        Task<CalendarEvent> SendPostEvent(string serviceAccountName, CalendarEvent eventRequest);

        /// <summary>
        /// Sends update calendar event request.
        /// </summary>
        /// <param name="serviceAccountName">Service Account Name.</param>
        /// <param name="eventRequest">Request body.</param>
        /// <param name="expand"> bool to controll response expansion</param>
        /// <returns>Event that was updated</returns>
        Task<CalendarEvent> SendPatchEvent(string serviceAccountName, CalendarEvent eventRequest, bool expand = true);

        /// <summary>
        /// Delete calendar event given the calendar event id.
        /// </summary>
        /// <param name="schedulerAccessToken">Scheduler access token.</param>
        /// <param name="id">Calendar event id.</param>
        /// <param name="isValidationRequired">Is validation required</param>
        /// <returns>Nothing is returned.</returns>
        Task DeleteCalendarEvent(string schedulerAccessToken, string id, bool isValidationRequired = false);

        /// <summary>
        /// Subscribes to a resource.
        /// </summary>
        /// <param name="subscriptionViewModel">subscription view model.</param>
        /// <param name="userAccessToken">User access token.</param>
        /// <param name="isSubscribingToEvents">Indicate whether subscribe to events or message</param>
        /// <returns>The subscription that was created.</returns>
        Task<SubscriptionViewModel> Subscribe(SubscriptionViewModel subscriptionViewModel, string userAccessToken, bool isSubscribingToEvents = true);

        /// <summary>
        /// Unsubscribes to a resource.
        /// </summary>
        /// <param name="subscriptionViewModel">subscription view model.</param>
        /// <param name="userAccessToken">User access token.</param>
        /// <returns>If deletion succeeds.</returns>
        Task<bool> Unsubscribe(SubscriptionViewModel subscriptionViewModel, string userAccessToken);

        /// <summary>
        /// Get inbox messages.
        /// </summary>
        /// <param name="messageId">message Id</param>
        /// <param name="userAccessToken">user access token.</param>
        /// <param name="serviceAccountEmail">Service account email</param>
        /// <returns>message that was retrieved</returns>
        Task<Message> GetMessageById(string messageId, string userAccessToken, string serviceAccountEmail);

        /// <summary>
        /// Get events by id.
        /// </summary>
        /// <param name="eventId">event Id</param>
        /// <param name="userAccessToken">user access token.</param>
        /// <param name="serviceAccountEmail">Service account email</param>
        /// <returns>message that was retrieved</returns>
        Task<CalendarEvent> GetEventById(string eventId, string userAccessToken, string serviceAccountEmail);

        /// <summary>
        /// Search User By Email.
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <param name="serviceAccountName">Service Account for token generation</param>
        /// <returns><see cref="Task"/></returns>
        Task<GraphUserResponse> SearchUserByEmail(string email, string serviceAccountName);

        /// <summary>
        /// Find meeting times in case of panel interview.
        /// </summary>
        /// <param name="findMeetingTimeRequest">request object</param>
        /// <returns><see cref="Task"/></returns>
        Task<FindMeetingTimeResponse> FindMeetingTimes(FindMeetingTimeRequest findMeetingTimeRequest);
    }
}
