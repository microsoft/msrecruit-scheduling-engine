namespace MS.GTA.CommonLibrary.UnitTest.TalentContracts.ScheduleService
{
    using Microsoft.Graph;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ConferenceInfo_Tests
    {
        [TestMethod]
        public void OperatorWithNullOnlineMeeting()
        {
            OnlineMeeting onlineMeeting = null;
            ConferenceInfo conferenceInfo = (ConferenceInfo)onlineMeeting;
            Assert.IsNull(conferenceInfo);
        }

        [TestMethod]
        public void OperatorWithValidOnlineMeeting()
        {
            MeetingParticipants participants = new MeetingParticipants();
            MeetingParticipantInfo participantInfo = new MeetingParticipantInfo
            {
                Identity = new IdentitySet
                {
                    User=new Identity
                    {
                        Id="Test Organizer Oid"
                    }
                },

                Upn = "TestUpn@xyz.com"
            };

            participants.Organizer = participantInfo;
            OnlineMeeting onlineMeeting = new OnlineMeeting
            {
                JoinWebUrl = "https://please.join.us/here",
                Id = "Meeting Id",
                Subject = "Test Subject",
                AudioConferencing = new AudioConferencing(),
                Participants = participants
            };

            ConferenceInfo conferenceInfo = (ConferenceInfo)onlineMeeting;

            Assert.IsTrue(conferenceInfo.Id.Equals(onlineMeeting.Id, StringComparison.Ordinal));
            Assert.IsTrue(conferenceInfo.JoinUrl.Equals(onlineMeeting.JoinWebUrl, StringComparison.Ordinal));
            Assert.IsTrue(conferenceInfo.Subject.Equals(onlineMeeting.Subject, StringComparison.Ordinal));
            Assert.IsTrue(conferenceInfo.OrganizerOid.Equals(onlineMeeting.Participants.Organizer.Identity.User.Id, StringComparison.Ordinal));
            Assert.IsNull(conferenceInfo.JobApplicationId);
        }
    }
}
