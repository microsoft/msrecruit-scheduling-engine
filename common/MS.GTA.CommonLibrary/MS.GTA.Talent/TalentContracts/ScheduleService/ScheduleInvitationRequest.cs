// <copyright file="ScheduleInvitationRequest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Talent.TalentContracts.ScheduleService
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="ScheduleInvitationRequest"/> class stores the schedule invitation request details.
    /// </summary>
    [DataContract]
    public class ScheduleInvitationRequest
    {
        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>
        /// The email subject.
        /// </value>
        [DataMember(Name = "subject", IsRequired = true)]
        [Required(ErrorMessage = " The subject is mandatory.", AllowEmptyStrings = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the content of the email.
        /// </summary>
        /// <value>
        /// The content of the email.
        /// </value>
        [DataMember(Name = "emailContent", IsRequired = true)]
        [Required(ErrorMessage = " The email content is mandatory.", AllowEmptyStrings = false)]
        public string EmailContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview title is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interview title is shared; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "isInterviewTitleShared", IsRequired = true)]
        public bool IsInterviewTitleShared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview schedule is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if thinterview schedule is shared; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "isInterviewScheduleShared", IsRequired = true)]
        public bool IsInterviewScheduleShared { get; set; }

        /// <summary>
        /// Gets or sets the primary email recipient.
        /// </summary>
        /// <value>
        /// The primary email recipient.
        /// </value>
        [DataMember(Name = "primaryEmailRecipient", IsRequired = true)]
        [Required(ErrorMessage = " The primary email recipients is mandatory.", AllowEmptyStrings = false)]
        public string PrimaryEmailRecipient { get; set; }

        /// <summary>
        /// Gets or sets the CC email address list.
        /// </summary>
        /// <value>
        /// The CC email address list.
        /// </value>
        [DataMember(Name = "ccEmailAddressList", IsRequired = false)]
        [MaxLength(20, ErrorMessage = "The email CC list cannot have more than 20 recipients.")]
        public List<string> CcEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets the shared schedules.
        /// </summary>
        /// <value>
        /// The shared schedules as instance of <see cref="List{CandidateScheduleCommunication}"/>.
        /// </value>
        [DataMember(Name = "sharedSchedules", IsRequired = false)]
        public List<CandidateScheduleCommunication> SharedSchedules { get; set; }
    }
}
