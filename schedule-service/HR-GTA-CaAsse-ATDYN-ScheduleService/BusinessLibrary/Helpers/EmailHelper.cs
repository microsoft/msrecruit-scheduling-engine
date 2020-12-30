//----------------------------------------------------------------------------
// <copyright file="EmailHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.Utils;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using TimeZoneConverter;
    using TimeZoneNames;

    /// <summary>
    /// Email helper
    /// </summary>
    public class EmailHelper : IEmailHelper
    {
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// The instance for <see cref="ILogger{EmailHelper}"/>.
        /// </summary>
        private readonly ILogger<EmailHelper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHelper"/> class
        /// </summary>
        /// <param name="scheduleQuery">schedule query instance</param>
        /// <param name="logger">The instance for <see cref="ILogger{EmailHelper}"/>.</param>
        public EmailHelper(
            IScheduleQuery scheduleQuery,
            ILogger<EmailHelper> logger)
        {
            this.logger = logger;
            this.scheduleQuery = scheduleQuery;
        }

        // TODO: Remove when selective schedule invite reaches production.

        /// <summary>
        /// Forms the Schedule Summary String for the Send to candidate email.
        /// </summary>
        /// <param name="schedules">Schedules of a given job id.</param>
        /// <param name="scheduleInvitationDetails">scheduleInvitationDetails.</param>
        /// <param name="timezone">time zone info</param>
        /// <returns>Table of Schedule Summaries.</returns>
        public async Task<string> GetScheduleSummary(List<MeetingInfo> schedules, ScheduleInvitationDetails scheduleInvitationDetails, Timezone timezone)
        {
            string scheduleSummary = string.Empty;

            if (!(schedules.Count <= 0 || timezone == null))
            {
                string timezoneName = null;
                TimeZoneInfo timeZoneInfo = null;
                IList<DateTime> dateList = new List<DateTime>();
                TimeZoneValues timezoneAbbreviations = null;
                Dictionary<DateTime, List<MeetingInfo>> groupedSchedules = new Dictionary<DateTime, List<MeetingInfo>>();

                if (!string.IsNullOrWhiteSpace(timezone?.TimezoneName))
                {
                    TZConvert.TryIanaToWindows(timezone.TimezoneName, out timezoneName);
                    timezoneAbbreviations = TZNames.GetAbbreviationsForTimeZone(timezone.TimezoneName, "en-US");
                }

                if (!string.IsNullOrWhiteSpace(timezoneName))
                {
                    try
                    {
                        timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneName);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogWarning(e, $"Exception in {nameof(this.GetScheduleSummary)}");
                    }
                }

                schedules.Sort((x, y) => DateTime.Compare(x.MeetingDetails[0].UtcStart, y.MeetingDetails[0].UtcStart));

                schedules.ForEach(s =>
                {
                    DateTime date = timeZoneInfo == null ? s.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset).Date :
                                TimeZoneInfo.ConvertTimeFromUtc(s.MeetingDetails[0].UtcStart, timeZoneInfo).Date;

                    dateList.Add(date);
                });

                var uniqueDates = dateList.Distinct();
                uniqueDates.ToList().ForEach(ud => groupedSchedules[ud] = new List<MeetingInfo>());

                IList<DateTime> finalDateList = new List<DateTime>();

                foreach (var schedule in schedules)
                {
                    if (schedule.MeetingDetails[0].Attendees.Any())
                    {
                        if (schedule.MeetingDetails[0].Attendees.Where(a => (a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.Declined
                                                     && a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.None)).Any())
                        {
                            DateTime dt = timeZoneInfo == null ? schedule.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset) :
                                TimeZoneInfo.ConvertTimeFromUtc(schedule.MeetingDetails[0].UtcStart, timeZoneInfo);
                            finalDateList.Add(dt.Date);
                            if (groupedSchedules.ContainsKey(dt.Date))
                            {
                                groupedSchedules[dt.Date].Add(schedule);
                            }
                        }
                    }
                    else
                    {
                        DateTime dt = timeZoneInfo == null ? schedule.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset) :
                                TimeZoneInfo.ConvertTimeFromUtc(schedule.MeetingDetails[0].UtcStart, timeZoneInfo);
                        finalDateList.Add(dt.Date);
                        if (groupedSchedules.ContainsKey(dt.Date))
                        {
                            groupedSchedules[dt.Date].Add(schedule);
                        }
                    }
                }

                if (finalDateList.Count > 0)
                {
                    scheduleSummary = "<style>.smryTable{font-family : Segoe UI; font-size: 14px; border-collapse: collapse; width: 100%;} .smryTr{border-bottom: 2px solid #dddddd;} .smryTd{text-align: center; padding-top: 6px; padding-bottom: 6px;}";
                    scheduleSummary = scheduleSummary + ".smryTh{font-family : Segoe UI; font-size: 14px; border: 2px solid #ffffff; background-color: #dddddd; text-align: center; padding: 6px;} </style>";
                }

                foreach (var date in finalDateList.Distinct())
                {
                    DateTime utcDate = timeZoneInfo == null ? date.AddMinutes(-timezone.UTCOffset) :
                                TimeZoneInfo.ConvertTimeToUtc(date, timeZoneInfo);

                    string timezoneAbbr = timeZoneInfo != null && timezoneAbbreviations != null ?
                        (timeZoneInfo.IsDaylightSavingTime(utcDate) ? timezoneAbbreviations.Daylight : timezoneAbbreviations.Standard)
                        : timezone?.TimezoneAbbr;

                    ////Interview date and timzone population
                    scheduleSummary = scheduleSummary + "<p><br>" + date.ToString("dddd") + " " + date.ToString("MMMM dd, yyyy") + " - " + timezoneAbbr + "-" + timezone?.TimezoneName + "<table class = \"smryTable\">";

                    scheduleSummary = scheduleSummary + "<tr class = \"smryTr\"><th class = \"smryTh\">Interview Details</th><th class = \"smryTh\">Interviewers</th><th class = \"smryTh\">Building Location</th><th class = \"smryTh\">Meeting Link</th></tr>";

                    if (groupedSchedules.ContainsKey(date))
                    {
                        var meetingList = groupedSchedules[date]?.Distinct();
                        scheduleSummary = await this.GetSummaryRows(scheduleInvitationDetails, timezone, scheduleSummary, meetingList, timeZoneInfo);

                        scheduleSummary = scheduleSummary + "</table></p>";
                    }
                }
            }

            return scheduleSummary;
        }

        /// <summary>
        /// Gets the schedule summary.
        /// </summary>
        /// <param name="schedules">The schedules.</param>
        /// <param name="scheduleInvitationRequest">The sinstance of <see cref="ScheduleInvitationRequest" />.</param>
        /// <param name="timezone">The timezone.</param>
        /// <returns>
        /// The schedule summary table.
        /// </returns>
        public async Task<string> GetScheduleSummaryAsync(List<MeetingInfo> schedules, ScheduleInvitationRequest scheduleInvitationRequest, Timezone timezone)
        {
            string scheduleSummary = string.Empty;
            StringBuilder scheduleSummaryBuilder = new StringBuilder(string.Empty);
            string timezoneName = null;
            TimeZoneInfo timeZoneInfo = null;
            TimeZoneValues timezoneAbbreviations = null;
            Dictionary<DateTime, List<MeetingInfo>> groupedSchedules = new Dictionary<DateTime, List<MeetingInfo>>();

            if (!string.IsNullOrWhiteSpace(timezone?.TimezoneName))
            {
                TZConvert.TryIanaToWindows(timezone.TimezoneName, out timezoneName);
                timezoneAbbreviations = TZNames.GetAbbreviationsForTimeZone(timezone.TimezoneName, "en-US");
            }

            if (!string.IsNullOrWhiteSpace(timezoneName))
            {
                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneName);
                }
                catch (Exception e)
                {
                    this.logger.LogWarning(e, $"Exception in {nameof(this.GetScheduleSummaryAsync)}");
                }
            }

            if (scheduleInvitationRequest != null && timezone != null && schedules.Any())
            {
                schedules.Sort((x, y) => DateTime.Compare(x.MeetingDetails[0].UtcStart, y.MeetingDetails[0].UtcStart));
                IList<DateTime> dateList = new List<DateTime>();

                schedules.ForEach(s =>
                {
                    DateTime date = timeZoneInfo == null ? s.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset).Date :
                                TimeZoneInfo.ConvertTimeFromUtc(s.MeetingDetails[0].UtcStart, timeZoneInfo).Date;

                    dateList.Add(date);
                });

                var uniqueDates = dateList.Distinct();
                uniqueDates.ToList().ForEach(ud => groupedSchedules[ud] = new List<MeetingInfo>());

                IList<DateTime> finalDateList = new List<DateTime>();

                foreach (var schedule in schedules)
                {
                    if (schedule.MeetingDetails[0].Attendees.Any())
                    {
                        if (schedule.MeetingDetails[0].Attendees.Where(a => (a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.Declined
                                                        && a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.None)).Any())
                        {
                            DateTime dt = timeZoneInfo == null ? schedule.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset) :
                            TimeZoneInfo.ConvertTimeFromUtc(schedule.MeetingDetails[0].UtcStart, timeZoneInfo);
                            finalDateList.Add(dt.Date);
                            if (groupedSchedules.ContainsKey(dt.Date))
                            {
                                groupedSchedules[dt.Date].Add(schedule);
                            }
                        }
                    }
                    else
                    {
                        DateTime dt = timeZoneInfo == null ? schedule.MeetingDetails[0].UtcStart.AddMinutes(timezone.UTCOffset) :
                            TimeZoneInfo.ConvertTimeFromUtc(schedule.MeetingDetails[0].UtcStart, timeZoneInfo);
                        finalDateList.Add(dt.Date);
                        if (groupedSchedules.ContainsKey(dt.Date))
                        {
                            groupedSchedules[dt.Date].Add(schedule);
                        }
                    }
                }

                if (finalDateList.Count > 0)
                {
                    scheduleSummaryBuilder.Append("<style>.smryTable{font-family : Segoe UI; font-size: 14px; border-collapse: collapse; width: 100%;} .smryTr{border-bottom: 2px solid #dddddd;} .smryTd{text-align: center; padding-top: 6px; padding-bottom: 6px;}");
                    scheduleSummaryBuilder.Append(".smryTh{font-family : Segoe UI; font-size: 14px; border: 2px solid #ffffff; background-color: #dddddd; text-align: center; padding: 6px;} </style>");
                }

                foreach (var date in finalDateList.Distinct())
                {
                    DateTime utcDate = timeZoneInfo == null ? date.AddMinutes(-timezone.UTCOffset) :
                                TimeZoneInfo.ConvertTimeToUtc(date, timeZoneInfo);

                    string timezoneAbbr = timeZoneInfo != null && timezoneAbbreviations != null ?
                        (timeZoneInfo.IsDaylightSavingTime(utcDate) ? timezoneAbbreviations.Daylight : timezoneAbbreviations.Standard)
                        : timezone?.TimezoneAbbr;

                    ////Interview date and timzone population
                    scheduleSummaryBuilder.Append("<p><br>" + date.ToString("dddd") + " " + date.ToString("MMMM dd, yyyy") + " - " + timezoneAbbr + "-" + timezone?.TimezoneName + "<table class = \"smryTable\"> ");

                    scheduleSummaryBuilder.Append("<tr class = \"smryTr\"><th class = \"smryTh\">Interview Details</th><th class = \"smryTh\">Interviewers</th><th class = \"smryTh\">Building Location</th><th class = \"smryTh\">Meeting Link</th></tr>");

                    if (groupedSchedules.ContainsKey(date))
                    {
                        var meetingList = groupedSchedules[date]?.Distinct();
                        await this.UpdateSummaryRowsAsync(schedules, scheduleInvitationRequest, timezone, scheduleSummaryBuilder, meetingList, timeZoneInfo).ConfigureAwait(false);
                        scheduleSummaryBuilder.Append("</table></p>");
                    }
                }
            }

            scheduleSummary = scheduleSummaryBuilder.ToString();
            return scheduleSummary;
        }

        private async Task<string> GetSummaryRows(ScheduleInvitationDetails scheduleInvitationDetails, Timezone timezone, string scheduleSummary, IEnumerable<MeetingInfo> meetingList, TimeZoneInfo timezoneInfo)
        {
            foreach (var meeting in meetingList)
            {
                string startTime = timezoneInfo == null ?
                    this.ParseTimebyTimeZoneOffset(meeting?.MeetingDetails[0]?.UtcStart.ToString(), timezone?.UTCOffset) :
                    TimeZoneInfo.ConvertTimeFromUtc(meeting.MeetingDetails[0].UtcStart, timezoneInfo).ToString("h:mm tt");

                string endTime = timezoneInfo == null ?
                    this.ParseTimebyTimeZoneOffset(meeting?.MeetingDetails[0]?.UtcEnd.ToString(), timezone?.UTCOffset) :
                    TimeZoneInfo.ConvertTimeFromUtc(meeting.MeetingDetails[0].UtcEnd, timezoneInfo).ToString("h:mm tt");

                ////Interview title row creation
                if (scheduleInvitationDetails != null && scheduleInvitationDetails.IsInterviewTitleShared)
                {
                    scheduleSummary = scheduleSummary + "<tr class = \"smryTr\"><td class = \"smryTd\">" + startTime + " - " + endTime + "<br>" + meeting?.MeetingDetails[0]?.Subject.ToString() + "</td>";
                }
                else
                {
                    scheduleSummary = scheduleSummary + "<tr class = \"smryTr\"><td class = \"smryTd\">" + startTime + " - " + endTime + "</td>";
                }

                scheduleSummary = scheduleSummary + "<td class = \"smryTd\">";

                if (meeting.MeetingDetails[0].Attendees.Any())
                {
                    foreach (var attendee in meeting.MeetingDetails[0].Attendees)
                    {
                        ////verifying the interviewer response status
                        if (attendee.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.None && attendee.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.Declined)
                        {
                            var interviewer = await this.scheduleQuery.GetWorker(attendee.User.Id);
                            scheduleSummary = scheduleSummary + interviewer?.FullName + "<br>";
                        }
                    }
                }
                else
                {
                    scheduleSummary = scheduleSummary + "<br>";
                }

                scheduleSummary = scheduleSummary + "</td>";
                ////Interview location details
                scheduleSummary = scheduleSummary + "<td class = \"smryTd\">" + EmailUtils.GetMeetingLocation(meeting?.MeetingDetails?[0].MeetingLocation);

                ////Teams link integration
                scheduleSummary = scheduleSummary + "<td class = \"smryTd\">" + EmailUtils.GetTeamsURL(meeting?.MeetingDetails?[0].Conference, startTime, endTime);

                scheduleSummary = scheduleSummary + "</td></tr>";
            }

            return scheduleSummary;
        }

        private async Task UpdateSummaryRowsAsync(List<MeetingInfo> schedules, ScheduleInvitationRequest scheduleInvitationRequest, Timezone timezone, StringBuilder scheduleSummaryBuilder, IEnumerable<MeetingInfo> meetingList, TimeZoneInfo timezoneInfo)
        {
            bool shareInterviewers = false;
            foreach (var meeting in meetingList)
            {
                string startTime = timezoneInfo == null ?
                    this.ParseTimebyTimeZoneOffset(meeting?.MeetingDetails[0]?.UtcStart.ToString(), timezone?.UTCOffset) :
                    TimeZoneInfo.ConvertTimeFromUtc(meeting.MeetingDetails[0].UtcStart, timezoneInfo).ToString("h:mm tt");

                string endTime = timezoneInfo == null ?
                    this.ParseTimebyTimeZoneOffset(meeting?.MeetingDetails[0]?.UtcEnd.ToString(), timezone?.UTCOffset) :
                    TimeZoneInfo.ConvertTimeFromUtc(meeting.MeetingDetails[0].UtcEnd, timezoneInfo).ToString("h:mm tt");

                if (scheduleInvitationRequest.IsInterviewTitleShared)
                {
                    scheduleSummaryBuilder.Append("<tr class = \"smryTr\"><td class = \"smryTd\">" + startTime + " - " + endTime + "<br>" + meeting?.MeetingDetails[0]?.Subject.ToString() + "</td>");
                }
                else
                {
                    scheduleSummaryBuilder.Append("<tr class = \"smryTr\"><td class = \"smryTd\">" + startTime + " - " + endTime + "</td>");
                }

                scheduleSummaryBuilder.Append("<td class = \"smryTd\">");
                shareInterviewers = scheduleInvitationRequest.SharedSchedules.Where(sch => sch.ScheduleId.Equals(meeting.Id, StringComparison.OrdinalIgnoreCase) && sch.IsInterviewerNameShared).Any();
                if (meeting.MeetingDetails[0].Attendees.Any())
                {
                    if (shareInterviewers)
                    {
                        foreach (var attendee in meeting.MeetingDetails[0].Attendees)
                        {
                            if (attendee.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.None && attendee.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.Declined)
                            {
                                var interviewer = await this.scheduleQuery.GetWorker(attendee.User.Id).ConfigureAwait(false);
                                scheduleSummaryBuilder.Append(interviewer?.FullName + "<br>");
                            }
                        }
                    }
                    else
                    {
                        scheduleSummaryBuilder.Append("...");
                    }
                }
                else
                {
                    scheduleSummaryBuilder.Append("<br>");
                }

                scheduleSummaryBuilder.Append("</td>");
                ////Interview location details
                scheduleSummaryBuilder.Append("<td class = \"smryTd\">" + EmailUtils.GetMeetingLocation(meeting?.MeetingDetails?[0].MeetingLocation));

                ////Teams link integration
                scheduleSummaryBuilder.Append("<td class = \"smryTd\">" + EmailUtils.GetTeamsURL(meeting?.MeetingDetails?[0].Conference, startTime, endTime));

                scheduleSummaryBuilder.Append("</td></tr>");
            }
        }

        /// <summary>
        /// Parse Time by TimeZone Offset
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="timeZoneOffset">Time Zone Offset</param>
        /// <returns>time</returns>
        private string ParseTimebyTimeZoneOffset(string dateTime, int? timeZoneOffset)
        {
            int offsetValue = 0;
            if (timeZoneOffset.HasValue)
            {
                offsetValue = timeZoneOffset.Value;
            }

            var interviewDateTime = DateTime.Parse(dateTime).ToUniversalTime().AddMinutes(offsetValue);
            return $"{interviewDateTime:h:mm tt}";
        }
    }
}
