// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ViewModelExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.ViewModelExtensions
{
    using System.Collections.Generic;
    using MS.GTA.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing;
    using MS.GTA.TalentEntities.Enum;
    using Contracts = MS.GTA.ScheduleService.Contracts.V1;
    using EmailTemplate = Common.Email.Contracts.EmailTemplate;

    /// <summary>The view model extensions for meeting info.</summary>
    public static partial class ViewModelExtensions
    {
        /// <summary>
        /// Meeting info to job application activity participants
        /// </summary>
        /// <param name="meetingInfo">Meeting info</param>
        /// <returns>JobApplication activity participants</returns>
        public static JobApplicationSchedule ToEntity(this Contracts.MeetingInfo meetingInfo)
        {
            var scheduleAvailability = new JobApplicationSchedule()
            {
                ScheduleID = meetingInfo.Id,
                MeetingInfoId = meetingInfo.Id,
                StartDateTime = meetingInfo.MeetingDetails?[0].UtcStart,
                EndDateTime = meetingInfo.MeetingDetails?[0].UtcEnd,
                BuildingAddress = meetingInfo.MeetingDetails?[0].MeetingLocation?.RoomList?.Address,
                BuildingName = meetingInfo.MeetingDetails?[0].MeetingLocation?.RoomList?.Name,
                BuildingResponseStatus = GetStatus(meetingInfo.MeetingDetails?[0].MeetingLocation?.RoomList?.Status),
                RoomAddress = meetingInfo.MeetingDetails?[0].MeetingLocation?.Room?.Address,
                RoomName = meetingInfo.MeetingDetails?[0].MeetingLocation?.Room?.Name,
                RoomResponseStatus = GetStatus(meetingInfo.MeetingDetails?[0].MeetingLocation?.Room?.Status),
                Subject = meetingInfo.MeetingDetails?[0].Subject,
                Description = meetingInfo.MeetingDetails?[0].Description,
                SkypeOnlineMeetingRequired = false,
                IsDirty = meetingInfo.MeetingDetails?[0].IsDirty ?? false,
                IsPrivateMeeting = meetingInfo.MeetingDetails?[0].IsPrivateMeeting ?? false,
                SkypeHtmlViewHref = meetingInfo.MeetingDetails?[0].SkypeOnlineMeeting?.Links?.HtmlView?.Href,
                SkypeJoinUrl = meetingInfo.MeetingDetails?[0].SkypeOnlineMeeting?.JoinUrl,
                SkypeOnlineMeetingId = meetingInfo.MeetingDetails?[0].SkypeOnlineMeeting?.OnlineMeetingId,
                CalendarEventId = meetingInfo.MeetingDetails?[0].CalendarEventId,
                ServiceAccountEmail = meetingInfo.MeetingDetails?[0].SchedulerAccountEmail,
                MeetingDetailId = meetingInfo.MeetingDetails?[0].Id,
                ScheduleRequester = meetingInfo.Requester?.ToWorker(),
                InterviewerTimeSlotID = meetingInfo.InterviewerTimeSlotId,
                ConferenceMeetingId = meetingInfo.MeetingDetails?[0].Conference?.Id,
                ConferenceProviderType = meetingInfo.MeetingDetails?[0].Conference?.Provider,
                Location = meetingInfo.MeetingDetails?[0].Location,
                Participants = GetParticipants(meetingInfo),
                OnlineMeetingRequired = meetingInfo.MeetingDetails[0].OnlineMeetingRequired
            };

            return scheduleAvailability;
        }

        /// <summary>
        /// Job application schedule to meetinginfo contract
        /// </summary>
        /// <param name="schedule">job application schedule object</param>
        /// <returns>meeting info contract</returns>
        public static Contracts.MeetingInfo ToContract(this JobApplicationSchedule schedule)
        {
            var meetingDetails = new MeetingInfo()
            {
                Id = schedule.MeetingInfoId,
                InterviewerTimeSlotId = schedule.InterviewerTimeSlotID,
                Requester = schedule.ScheduleRequester.ToGraphPerson(),
                MeetingDetails = GetMeetingDetails(schedule),
                UserGroups = GetUserGroups(schedule),
                ScheduleEventId = schedule.CalendarEventId,
                ScheduleStatus = schedule.ScheduleStatus
            };

            return meetingDetails;
        }

        /// <summary>
        /// email template to contract
        /// </summary>
        /// <param name="templateModel">jemail template</param>
        /// <returns>meeting info contract</returns>
        public static EmailTemplate ToContract(this FalconEmailTemplate templateModel)
        {
            return new EmailTemplate
            {
                Id = templateModel.Id,
                TemplateName = templateModel.TemplateName,
                AppName = templateModel.AppName,
                TemplateType = templateModel.TemplateType,
                IsGlobal = false,
                To = templateModel.To,
                Cc = templateModel.Cc,
                Bcc = templateModel.Bcc,
                Subject = templateModel.Subject,
                Header = templateModel.Header,
                Body = templateModel.Body,
                Closing = templateModel.Closing,
                Footer = templateModel.Footer,
                IsDefault = templateModel.IsDefault,
                Creator = templateModel.CreatedBy,
                Language = templateModel.Language,
            };
        }

        /// <summary>
        /// Graph Person to worker
        /// </summary>
        /// <param name="graphPerson">Graph Person</param>
        /// <returns>Worker</returns>
        public static Worker ToWorker(this Contracts.GraphPerson graphPerson)
        {
            var worker = new Worker();

            if (graphPerson != null)
            {
                worker.Id = worker.OfficeGraphIdentifier = graphPerson.Id;
                worker.EmailPrimary = graphPerson.Email;
                worker.Name = new PersonName()
                {
                    GivenName = graphPerson.GivenName,
                    Surname = graphPerson.Surname,
                    MiddleName = graphPerson.Name
                };
                worker.FullName = graphPerson.Name;
                worker.Profession = graphPerson.Title;
            }

            return worker;
        }

        /// <summary>
        /// worker to Graph Person
        /// </summary>
        /// <param name="worker">Worker</param>
        /// <returns>graph person</returns>
        public static Contracts.GraphPerson ToGraphPerson(this Worker worker)
        {
            var graphObject = new Contracts.GraphPerson();

            if (worker != null)
            {
                graphObject.Email = worker?.EmailPrimary;
                graphObject.GivenName = worker?.Name?.GivenName;
                graphObject.Id = worker?.OfficeGraphIdentifier;
                graphObject.Name = worker?.Name?.GivenName;
                graphObject.Surname = worker?.Name?.Surname;
                graphObject.UserPrincipalName = worker?.Alias;
                graphObject.Title = worker?.Profession;
            }

            return graphObject;
        }

        /// <summary>
        /// get status
        /// </summary>
        /// <param name="status">status</param>
        /// <returns>meeting info contract</returns>
        public static InvitationResponseStatus GetStatus(InvitationResponseStatus? status)
        {
            if (status != null)
            {
                return status.ToContractEnum<InvitationResponseStatus>();
            }
            else
            {
                return InvitationResponseStatus.None;
            }
        }

        private static List<JobApplicationScheduleParticipant> GetParticipants(Contracts.MeetingInfo meetingInfo)
        {
            List<JobApplicationScheduleParticipant> jobApplicationScheduleParticipants = new List<JobApplicationScheduleParticipant>();

            if (meetingInfo.UserGroups != null && meetingInfo.UserGroups.Users != null)
            {
                foreach (var item in meetingInfo.MeetingDetails[0]?.Attendees)
                {
                    if (item.User != null)
                    {
                        JobApplicationScheduleParticipant participant = new JobApplicationScheduleParticipant()
                        {
                            OID = item.User.Id,
                            ParticipantMetadata = item.User.Email,
                            ParticipantComments = item.User.Comments,
                            ParticipantStatus = item.User.InvitationResponseStatus
                        };

                        jobApplicationScheduleParticipants.Add(participant);
                    }
                }
            }

            return jobApplicationScheduleParticipants;
        }

        private static List<Contracts.MeetingDetails> GetMeetingDetails(JobApplicationSchedule schedule)
        {
            List<Contracts.MeetingDetails> meetingDetails = new List<Contracts.MeetingDetails>();

            Contracts.MeetingDetails meeting = new Contracts.MeetingDetails();

            meeting.CalendarEventId = schedule.CalendarEventId;
            meeting.Id = schedule.MeetingDetailId;
            meeting.IsDirty = schedule.IsDirty;
            meeting.IsPrivateMeeting = schedule.IsPrivateMeeting;
            meeting.SchedulerAccountEmail = schedule.ServiceAccountEmail;
            meeting.SkypeOnlineMeeting = new Contracts.SkypeSchedulerResponse();
            meeting.SkypeOnlineMeeting.OnlineMeetingId = schedule.SkypeOnlineMeetingId;
            meeting.SkypeOnlineMeeting.OnlineMeetingUri = schedule.SkypeJoinUrl;
            meeting.SkypeOnlineMeetingRequired = schedule.SkypeOnlineMeetingRequired;
            meeting.OnlineMeetingRequired = schedule.OnlineMeetingRequired;
            meeting.Subject = schedule.Subject;
            meeting.Description = schedule.Description;
            meeting.UnknownFreeBusyTime = schedule.UnknownFreeBusyTime;
            meeting.IsInterviewScheduleShared = schedule.IsInterviewScheduleShared;
            meeting.IsInterviewerNameShared = schedule.IsInterviewerNameShared;

            meeting.MeetingLocation = new InterviewMeetingLocation()
            {
                RoomList = new Room()
                {
                    Address = schedule.BuildingAddress,
                    Name = schedule.BuildingName,
                    Status = schedule.BuildingResponseStatus
                },
                Room = new Room()
                {
                    Address = schedule.RoomAddress,
                    Name = schedule.RoomName,
                    Status = schedule.RoomResponseStatus
                }
            };

            if (schedule.StartDateTime.HasValue)
            {
                meeting.UtcStart = schedule.StartDateTime.Value.ToUniversalTime();
            }

            if (schedule.EndDateTime.HasValue)
            {
                meeting.UtcEnd = schedule.EndDateTime.Value.ToUniversalTime();
            }

            meeting.Attendees = GetMeetingAttendees(schedule);
            meeting.Location = schedule.Location;
            meeting.Conference = GetMeetingConference(schedule);

            meetingDetails.Add(meeting);

            return meetingDetails;
        }

        private static ConferenceInfo GetMeetingConference(JobApplicationSchedule schedule)
        {
            ConferenceInfo meeting = new ConferenceInfo();

            meeting.Audio = new Talent.TalentContracts.ScheduleService.Conferencing.AudioInfo()
            {
                TollNumber = schedule.OnlineTeamMeeting?.TollNumber,
                TollFreeNumber = schedule.OnlineTeamMeeting?.TollFreeNumbers,
                ConferenceId = schedule.OnlineTeamMeeting?.ConferenceId,
                DialInUrl = schedule.OnlineTeamMeeting?.QuickDial
            };
            meeting.JoinUrl = schedule.OnlineTeamMeeting?.JoinUrl;
            meeting.JoinInfo = schedule.OnlineMeetingContent?.Replace("\n\r", null);
            return meeting;
        }

        private static List<Contracts.Attendee> GetMeetingAttendees(JobApplicationSchedule schedule)
        {
            List<Contracts.Attendee> attendees = new List<Contracts.Attendee>();
            if (schedule.Participants != null)
            {
                foreach (var item in schedule.Participants)
                {
                    Contracts.Attendee attendee = new Contracts.Attendee();

                    Contracts.GraphPerson graphPerson = new Contracts.GraphPerson();
                    graphPerson.Id = item.OID;
                    graphPerson.Email = item.ParticipantMetadata;
                    attendee.User = graphPerson;
                    attendee.ResponseStatus = item.ParticipantStatus;
                    attendee.ResponseComment = item.ParticipantComments;
                    if (item.ParticipantResponseDateTime.HasValue)
                    {
                        attendee.ResponseDateTime = item.ParticipantResponseDateTime.Value;
                    }

                    attendees.Add(attendee);
                }
            }

            return attendees;
        }

        private static Contracts.UserGroup GetUserGroups(JobApplicationSchedule schedule)
        {
            Contracts.UserGroup userGroup = new Contracts.UserGroup();
            userGroup.Users = new List<Contracts.GraphPerson>();
            if (schedule.Participants != null)
            {
                foreach (var item in schedule.Participants)
                {
                    Contracts.GraphPerson graphPerson = new Contracts.GraphPerson();
                    graphPerson.Id = item.OID;

                    graphPerson.Email = item.ParticipantMetadata;
                    ////graphPerson.Comments = item.ParticipantComments;
                    graphPerson.InvitationResponseStatus = item.ParticipantStatus;

                    userGroup.Users.Add(graphPerson);
                }
            }

            return userGroup;
        }
    }
}
