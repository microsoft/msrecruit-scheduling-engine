//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.UnitTest.TalentContracts.ScheduleService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.Graph;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TA.CommonLibrary.ScheduleService.Contracts.V1;
    using TA.CommonLibrary.Talent.TalentContracts.ScheduleService.Conferencing;
    using MI = TA.CommonLibrary.ScheduleService.Contracts.V1;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ConferenceRequest_Tests
    {
        [TestMethod]
        public void OperatorWithNullConferenceRequest()
        {
            ConferenceRequest conferenceRequest = null;
            Microsoft.Graph.OnlineMeeting onlineMeeting = (Microsoft.Graph.OnlineMeeting)conferenceRequest;
            Assert.IsNull(onlineMeeting);
        }

        [TestMethod]
        public void OperatorWithValidConferenceResuest()
        {
            DateTime dateTime = DateTime.Now;
            ConferenceRequest conferenceRequest = new ConferenceRequest
            {
                StartTime = dateTime.AddMinutes(30),
                EndTime = dateTime.AddHours(1),
                Subject = "Test Meeting",
                ParticipantEmailAddresses = new List<string> { "abc@xyz.com" }
            };

            Microsoft.Graph.OnlineMeeting onlineMeeting = (Microsoft.Graph.OnlineMeeting)conferenceRequest;
            Assert.IsTrue(onlineMeeting.StartDateTime == dateTime.AddMinutes(30));
            Assert.IsTrue(onlineMeeting.EndDateTime == dateTime.AddHours(1));
            Assert.IsTrue(onlineMeeting.Subject.Equals(conferenceRequest.Subject, StringComparison.Ordinal));
            Assert.IsTrue(onlineMeeting.Participants.Attendees.Count() == conferenceRequest.ParticipantEmailAddresses.Count);
        }

        [TestMethod]
        public void OperatorWithValidMeetingInfo()
        {
            DateTime dateTime = DateTime.UtcNow;
            UserGroup userGroup = new UserGroup();
            userGroup.Users = new List<GraphPerson>
            {
                new GraphPerson
                {
                    Id = "User Id",
                    Name = "My Name",
                    Title = "SOFTWARE ENGINEER II"
                }
            };

            MI.Attendee attendee = new MI.Attendee();
            attendee.User = new GraphPerson
            {
                Id = "User Id",
                Name = "My Name",
                Title = "SOFTWARE ENGINEER II",
                Email = "abc@xyz.com"
            };

            MI.MeetingDetails meetingDetails = new MeetingDetails
            {
                Id = "Meeting Id",
                UtcStart = dateTime.AddMinutes(29),
                UtcEnd = dateTime.AddMinutes(59),
                Subject = "Meeting Subject",
                Attendees = new List<MI.Attendee>
                {
                    attendee
                }
            };

            MI.MeetingInfo meetingInfo = new MI.MeetingInfo
            {
                Id = "Meeting Id",
                UserGroups = userGroup,
                MeetingDetails = new List<MeetingDetails>()
            };

            meetingInfo.MeetingDetails.Add(meetingDetails);

            ConferenceRequest conferenceRequest = (ConferenceRequest)meetingInfo;
            Assert.IsTrue(conferenceRequest.StartTime == dateTime.AddMinutes(29));
            Assert.IsTrue(conferenceRequest.EndTime == dateTime.AddMinutes(59));
            Assert.IsTrue(meetingDetails.Subject.Equals(conferenceRequest.Subject, StringComparison.Ordinal));
            Assert.IsTrue(meetingDetails.Attendees.Count() == conferenceRequest.ParticipantEmailAddresses.Count);
            Assert.IsTrue(meetingDetails.Id.Equals(conferenceRequest.ScheduleId, StringComparison.Ordinal));
            Assert.IsNull(conferenceRequest.JobApplicationId);
        }

        [TestMethod]
        public void OperatorWithNullMeetingInfo()
        {
            MI.MeetingInfo meetingInfo = null;
            ConferenceRequest conferenceRequest = (ConferenceRequest)meetingInfo;
            Assert.IsNull(conferenceRequest);
        }

        [TestMethod]
        public void OperatorWithEmptyDetailsInMeetingInfo()
        {
            UserGroup userGroup = new UserGroup();
            userGroup.Users = new List<GraphPerson>
            {
                new GraphPerson
                {
                    Id = "User Id",
                    Name = "My Name",
                    Title = "SOFTWARE ENGINEER II"
                }
            };

            MI.MeetingInfo meetingInfo = new MI.MeetingInfo
            {
                Id = "Meeting Id",
                UserGroups = userGroup,
                MeetingDetails = new List<MeetingDetails>()
            };

            ConferenceRequest conferenceRequest = (ConferenceRequest)meetingInfo;
            Assert.IsNull(conferenceRequest);
        }
    }
}
