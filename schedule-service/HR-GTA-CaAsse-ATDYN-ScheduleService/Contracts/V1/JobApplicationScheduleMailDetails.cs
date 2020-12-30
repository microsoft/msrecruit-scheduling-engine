//----------------------------------------------------------------------------
// <copyright file="JobApplicationScheduleMailDetails.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    /// <summary>
    /// Holds the jobapplication schedule mail details
    /// </summary>
    [DataContract]
    public class JobApplicationScheduleMailDetails : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the ScheduleID
        /// </summary>
        [DataMember(Name = "ScheduleID", IsRequired = false)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets the mail sending date time
        /// </summary>
        [DataMember(Name = "MailSendDateTime", IsRequired = false)]
        public string MailSendDateTime { get; set; }

        /// <summary>
        /// Gets or sets the mail subject
        /// </summary>
        [DataMember(Name = "ScheduleMailSubject", IsRequired = false)]
        public string ScheduleMailSubject { get; set; }

        /// <summary>
        /// Gets or sets the mail description
        /// </summary>
        [DataMember(Name = "ScheduleMailDescription", IsRequired = false)]
        public string ScheduleMailDescription { get; set; }

        /// <summary>
        /// Gets or sets the mail PrimaryEmailRecipients
        /// </summary>
        [DataMember(Name = "PrimaryEmailRecipients", IsRequired = false)]
        public List<string> PrimaryEmailRecipients { get; set; }

        /// <summary>
        /// Gets or sets the mail BccEmailAddressList
        /// </summary>
        [DataMember(Name = "BccEmailAddressList", IsRequired = false)]
        public List<string> BccEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets the mail CCEmailAddressList
        /// </summary>
        [DataMember(Name = "CCEmailAddressList", IsRequired = false)]
        public List<string> CCEmailAddressList { get; set; }
    }
}
