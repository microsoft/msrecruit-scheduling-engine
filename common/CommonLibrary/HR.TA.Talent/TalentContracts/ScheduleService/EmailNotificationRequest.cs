//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.TalentContracts.ScheduleService
{
    using HR.TA..ScheduleService.Contracts.V1;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="EmailNotificationRequest"/> contains the information to whom email notification is sent.
    /// </summary>
    [DataContract]
    public class EmailNotificationRequest
    {
        /// <summary>
        /// Gets or sets the job application identifier.
        /// </summary>
        /// <value>
        /// The job application identifier.
        /// </value>
        [DataMember(Name = "jobApplicationId", IsRequired = true, EmitDefaultValue = false)]
        [Required(ErrorMessage = "The job application Id is required.", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "The job application Id cannot exceed 50 characters.")]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the email body content.
        /// </summary>
        /// <value>
        /// The email body content.
        /// </value>
        [DataMember(Name = "emailBody", IsRequired = true, EmitDefaultValue = false)]
        [Required(ErrorMessage = "Invalid email body.", AllowEmptyStrings = false)]
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets the email footer.
        /// </summary>
        /// <value>
        /// The email footer.
        /// </value>
        [DataMember(Name = "emailFooter", IsRequired = false, EmitDefaultValue = false)]
        public string EmailFooter { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>
        /// The email subject.
        /// </value>
        [DataMember(Name = "subject", IsRequired = true, EmitDefaultValue = false)]
        [Required(ErrorMessage = "Invalid subject.")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the email recipients in To.
        /// </summary>
        /// <value>
        /// The instance of <see cref="List{GraphPerson}"/> as email recipients in To.
        /// </value>
        [DataMember(Name = "mailTo", IsRequired = true, EmitDefaultValue = false)]
        [Required(ErrorMessage = "The 'To' members are missing.")]
        [MinLength(1, ErrorMessage = "The 'To' members are not specified.")]
        public List<GraphPerson> MailTo { get; set; }

        /// <summary>
        /// Gets or sets the email recipients in CC.
        /// </summary>
        /// <value>
        /// The instance of <see cref="List{GraphPerson}"/> as email recipients in CC.
        /// </value>
        [DataMember(Name = "mailCC", IsRequired = false, EmitDefaultValue = false)]
        public List<GraphPerson> MailCC { get; set; }
    }
}
