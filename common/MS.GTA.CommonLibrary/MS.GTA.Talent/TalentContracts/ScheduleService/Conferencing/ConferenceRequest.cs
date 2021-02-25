//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.ScheduleService.Conferencing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using Microsoft.Graph;
    using MI = GTA.ScheduleService.Contracts.V1;


    /// <summary>
    /// The <see cref="ConferenceRequest"/> class holds conference request information.
    /// </summary>
    [DataContract]
    public class ConferenceRequest
    {
        /// <summary>
        /// Gets or sets the conference UTC start time.
        /// </summary>
        /// <value>
        /// The conference UTC start time.
        /// </value>
        [Required(ErrorMessage = "The conference start time is mandatory.")]
        [DataMember(Name = "startTime", IsRequired = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the UTC end time of conference.
        /// </summary>
        /// <value>
        /// The conference UTC end time.
        /// </value>
        [Required(ErrorMessage = "The conference end time is mandatory.")]
        [DataMember(Name ="endTime", IsRequired = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting subject.
        /// </summary>
        /// <value>
        /// The conference meeting subject.
        /// </value>
        [Required(ErrorMessage = "The conference subject is mandatory.", AllowEmptyStrings = false)]
        [DataMember(Name = "subject", IsRequired = true)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the participant email addresses.
        /// </summary>
        /// <value>
        /// The participant email addresses.
        /// </value>
        [DataMember(Name = "participantEmailAddresses", IsRequired = true)]
        [Required(ErrorMessage = "The participant email addresses are mandatory.")]
        [MinLength(1, ErrorMessage = "The participants are missing.")]
        [MaxLength(10, ErrorMessage = "The number of participants exceeds 10.")]
        public List<string> ParticipantEmailAddresses { get; set; }

        /// <summary>
        /// Gets or sets the job application identifier (non-serializable property).
        /// </summary>
        /// <value>
        /// The job application identifier (non-serializable property).
        /// </value>
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier (non-serializable property).
        /// </summary>
        /// <value>
        /// The schedule identifier ( (non-serializable property).
        /// </value>
        public string ScheduleId { get; set; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ConferenceRequest"/> to <see cref="OnlineMeeting"/>.
        /// </summary>
        /// <param name="conferenceRequest">The instance of <see cref="ConferenceRequest"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator OnlineMeeting(ConferenceRequest conferenceRequest)
        {
            OnlineMeeting onlineMeeting = null;
            if (conferenceRequest != null)
            {
                onlineMeeting = new OnlineMeeting
                {
                    StartDateTime = conferenceRequest.StartTime,
                    EndDateTime = conferenceRequest.EndTime,
                    Subject = conferenceRequest.Subject,
                    Participants = new MeetingParticipants()
                };

                if (conferenceRequest.ParticipantEmailAddresses != null)
                {
                    onlineMeeting.Participants.Attendees = new List<MeetingParticipantInfo>();
                    foreach (string particpantEmail in conferenceRequest.ParticipantEmailAddresses)
                    {
                        onlineMeeting.Participants.Attendees = onlineMeeting.Participants.Attendees.Append(new MeetingParticipantInfo
                        {
                            Identity = new IdentitySet
                            {
                                User = new Identity
                                {
                                    Id = particpantEmail
                                }
                            },
                            Upn = particpantEmail
                        });
                    }
                }
            }

            return onlineMeeting;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="MI.MeetingInfo"/> to <see cref="ConferenceRequest"/>.
        /// </summary>
        /// <param name="meetingInfo">The instance of <see cref="MI.MeetingInfo"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator ConferenceRequest(MI.MeetingInfo meetingInfo)
        {
            ConferenceRequest conferenceRequest = null;
            MI.MeetingDetails meetingDetails = null;
            if(meetingInfo?.MeetingDetails != null && meetingInfo.MeetingDetails.Any())
            {
                meetingDetails = meetingInfo.MeetingDetails[0];
                conferenceRequest = new ConferenceRequest
                {
                    StartTime = meetingDetails.UtcStart,
                    EndTime = meetingDetails.UtcEnd,
                    Subject = meetingDetails.Subject,
                    ScheduleId = meetingDetails.Id,
                    ParticipantEmailAddresses = new List<string>()
                };

                meetingDetails.Attendees?.ForEach((attendee) =>
                {
                    if (!string.IsNullOrWhiteSpace(attendee.User?.Email))
                    {
                        conferenceRequest.ParticipantEmailAddresses.Add(attendee.User.Email);
                    }
                });
            }

            return conferenceRequest;
        }
    }
}
