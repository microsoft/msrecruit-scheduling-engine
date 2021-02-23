//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ScheduleService.BusinessLibrary;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing;
    using Email = MS.GTA.Common.Email;

    /// <summary>
    /// Contract utilities
    /// </summary>
    public static class EmailUtils
    {
        /// <summary>Gets gets or sets the tracer instance.</summary>
        private static ITraceSource Trace => ScheduleServiceBusinessLibraryTracer.Instance;

        /// <summary>
        /// Get User Email From Token
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <returns>service account email</returns>
        public static string GetUserEmailFromToken(string accessToken)
        {
            Contract.CheckValue(accessToken, nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();
            var user = handler.ReadToken(accessToken) as JwtSecurityToken;
            return user?.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
        }

        /// <summary>
        /// Check for retry on graph exception
        /// </summary>
        /// <param name="statusCode">Http status Code</param>
        /// <returns>Is retry required</returns>
        public static bool ShouldRetryOnGraphException(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.ServiceUnavailable
                        || (int)statusCode == Email.Constants.TooManyRequestCode
                        || statusCode == HttpStatusCode.GatewayTimeout
                        || statusCode == HttpStatusCode.PreconditionFailed;
        }

        /// <summary>
        /// Exponential Delay
        /// </summary>
        /// <param name="response">Http response</param>
        /// <param name="retryAttempt">Retry count</param>
        /// <returns>Task</returns>
        public static async Task ExponentialDelay(HttpResponseMessage response, int retryAttempt)
        {
            var delayInSeconds = (1d / 2d) * (Math.Pow(2d, retryAttempt) - 1d);

            var waitTimeSpan = response?.Headers?.RetryAfter?.Delta;
            var defaultTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            if (waitTimeSpan == null || waitTimeSpan <= TimeSpan.FromSeconds(0))
            {
                waitTimeSpan = defaultTimeSpan;
            }

            Trace.TraceInformation($"Delay thread with {waitTimeSpan ?? defaultTimeSpan} seconds before retry");

            await Task.Delay(waitTimeSpan ?? defaultTimeSpan);

            Trace.TraceInformation($"Processing retry  after {waitTimeSpan ?? defaultTimeSpan} delay");
        }

        /// <summary>
        /// Gets the meeting location in string format.
        /// </summary>
        /// <param name="meetingLocation">The instance of <see cref="InterviewMeetingLocation"/>.</param>
        /// <returns>The meeting location.</returns>
        public static string GetMeetingLocation(InterviewMeetingLocation meetingLocation)
        {
            string location = string.Empty;
            if (meetingLocation != null)
            {
                if (string.IsNullOrWhiteSpace(meetingLocation.RoomList?.Name))
                {
                    location = meetingLocation.Room?.Name;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(meetingLocation.Room?.Name))
                    {
                        location = meetingLocation.RoomList?.Name + " - " + meetingLocation.Room?.Name;
                    }
                    else
                    {
                        location = meetingLocation.RoomList?.Name;
                    }
                }
            }

            return location;
        }

        /// <summary>
        /// Get the teams meeting url using conference meeting
        /// </summary>
        /// <param name="conferenceMeeting">conference meeting</param>
        /// <param name="startTime">start time</param>
        /// <param name="endTime">end time</param>
        /// <returns>teams meeting url</returns>
        public static string GetTeamsURL(ConferenceInfo conferenceMeeting, string startTime, string endTime)
        {
            string meetingLink = string.Empty;
            if (conferenceMeeting != null && !string.IsNullOrWhiteSpace(conferenceMeeting.JoinUrl))
            {
                meetingLink = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" target =\"_blank\"><u><strong>Meeting link for {1} - {2}</strong></u></a>", conferenceMeeting.JoinUrl, startTime, endTime);
            }

            return meetingLink;
        }

        /// <summary>
        /// Get teams meeting information
        /// </summary>
        /// <param name="conferenceMeeting">conference meeting</param>
        /// <returns>meeting info</returns>
        public static string GetTeamMeetingInfo(ConferenceInfo conferenceMeeting)
        {
            string mailContent = string.Empty;
            if (conferenceMeeting == null)
            {
                return mailContent;
            }

            if (string.IsNullOrWhiteSpace(conferenceMeeting.JoinInfo))
            {
                mailContent = "<hr/>";
                mailContent = mailContent + string.Format(CultureInfo.InvariantCulture, "<p><a href=\"{0}\" target =\"_blank\"><u><strong>Join Microsoft Teams Meeting</strong></u></a></p>", conferenceMeeting.JoinUrl);

                if (conferenceMeeting.Audio != null)
                {
                    if (!string.IsNullOrWhiteSpace(conferenceMeeting.Audio.TollNumber))
                    {
                        mailContent = mailContent + string.Format(CultureInfo.InvariantCulture, "<p><a href= \"tel: +1 {0},, {1}#\" target =\"_blank\"><u>{0}</u></a></p>", conferenceMeeting.Audio.TollNumber, conferenceMeeting.Audio.ConferenceId);
                    }

                    if (!string.IsNullOrWhiteSpace(conferenceMeeting.Audio.TollFreeNumber))
                    {
                        mailContent = mailContent + string.Format(CultureInfo.InvariantCulture, "<p><a href =\"tel: {0},,, {1}# \" target =\"_blank\"><u>{0}</u></a>(Toll-free)</p>", conferenceMeeting.Audio.TollFreeNumber, conferenceMeeting.Audio.ConferenceId);
                    }

                    if (!string.IsNullOrWhiteSpace(conferenceMeeting.Audio.ConferenceId))
                    {
                        mailContent = mailContent + string.Format(CultureInfo.InvariantCulture, "<p>Conference ID: {0}#</p>", conferenceMeeting.Audio?.ConferenceId);
                    }
                }

                mailContent = mailContent + "<hr/><br/>";
            }
            else
            {
                mailContent = "<br/>" + conferenceMeeting.JoinInfo;
            }

            return mailContent;
        }

        /// <summary>
        /// Prepares the email attachment.
        /// </summary>
        /// <param name="emailAttachments"> Email Attachments.</param>
        /// <returns>The instance of <see cref="NotificationAttachment"/>.</returns>
        public static IList<NotificationAttachment> PrepareEmailAttachments(Talent.TalentContracts.InterviewService.FileAttachmentRequest emailAttachments)
        {
            IList<NotificationAttachment> attachments = new List<NotificationAttachment>();

            if (emailAttachments != null && emailAttachments.Files != null && emailAttachments.Files.Count > 0)
            {
                IList<string> labels = emailAttachments.FileLabels.ToList();
                IFormFileCollection files = emailAttachments.Files;

                var filesWithLabels = labels.Zip(files, (label, file) => new { Key = label, Value = file })
                            .ToDictionary(x => x.Key, x => x.Value);

                foreach (var pair in filesWithLabels)
                {
                    if (pair.Value.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            pair.Value.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            string fileBase64 = Convert.ToBase64String(fileBytes);
                            var fileNameSplit = pair.Value.FileName.Split(new char[] { '.' });
                            if (fileNameSplit != null || fileNameSplit.Length >= 2)
                            {
                                string fileExtension = fileNameSplit[fileNameSplit.Length - 1];
                                attachments.Add(new NotificationAttachment { FileBase64 = fileBase64, FileName = pair.Key + "." + fileExtension });
                            }
                        }
                    }
                }
            }

            return attachments;
        }
    }
}
