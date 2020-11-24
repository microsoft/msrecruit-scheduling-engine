// <copyright file="SkypeOnlineMeetingExtension.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Skype for business online meeting extension
    /// </summary>
    [DataContract]
    public class SkypeOnlineMeetingExtension
    {
        /// <summary>
        /// Gets or sets the Online meeting Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting type
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting conference link
        /// </summary>
        [DataMember(Name = "onlineMeetingConfLink")]
        public string OnlineMeetingConfLink { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting external link
        /// </summary>
        [DataMember(Name = "onlineMeetingExternalLink")]
        public string OnlineMeetingExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting internal link
        /// </summary>
        [DataMember(Name = "onlineMeetingInternalLink")]
        public string OnlineMeetingInternalLink { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting setting
        /// </summary>
        [DataMember(Name = "ucMeetingSetting")]
        public string UCMeetingSetting { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting in band
        /// </summary>
        [DataMember(Name = "ucInband")]
        public string UCInband { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting capabilities
        /// </summary>
        [DataMember(Name = "ucCapabilities")]
        public string UCCapabilities { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting participant passcode
        /// </summary>
        [DataMember(Name = "participantPassCode")]
        public string ParticipantPassCode { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting toll number
        /// </summary>
        [DataMember(Name = "tollNumber")]
        public string TollNumber { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting toll free number
        /// </summary>
        [DataMember(Name = "tollFreeNumber")]
        public string TollFreeNumber { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting links
        /// </summary>
        [DataMember(Name = "_links")]
        public SkypeLinks Links { get; set; }
    }
}
