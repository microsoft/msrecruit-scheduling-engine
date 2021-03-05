//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum that tells about Feedback Reminder Delay.
    /// </summary>
    [DataContract]
    public enum FeedbackReminderDelay
    {
        /// <summary>
        /// No Reminder
        /// </summary>
        None = 0,

        /// <summary>
        /// 12 Hours
        /// </summary>
        HalfDay = 1,

        /// <summary>
        /// 1 day
        /// </summary>
        OneDay = 2,

        /// <summary>
        /// 2 days
        /// </summary>
        TwoDays = 3,

        /// <summary>
        /// 3 days
        /// </summary>
        ThreeDays = 4,

        /// <summary>
        /// 1 week
        /// </summary>
        OneWeek = 5,
    }

    /// <summary>
    /// Configuration for Feedback activity.
    /// </summary>
    [DataContract]
    public class FeedbackConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether partcipants are hiring team or not.
        /// </summary>
        [DataMember(Name = "inheritFromHiringTeam", IsRequired = false, EmitDefaultValue = false)]
        public bool InheritFromHiringTeam { get; set; }

        /// <summary>
        /// Gets or sets a value whether you can view other feedbacks before submitting your own.
        /// </summary>
        [DataMember(Name = "viewFeedbackBeforeSubmitting", IsRequired = false, EmitDefaultValue = false)]
        public bool ViewFeedbackBeforeSubmitting { get; set; }

        /// <summary>
        /// Gets or sets a value whether you can edit your feedback after submission.
        /// </summary>
        [DataMember(Name = "editFeedbackAfterSubmitting", IsRequired = false, EmitDefaultValue = false)]
        public bool EditFeedbackAfterSubmitting { get; set; }

        /// <summary>
        /// Gets or sets external job opening source.
        /// </summary>
        [DataMember(Name = "feedbackReminderDelay", IsRequired = false, EmitDefaultValue = false)]
        public FeedbackReminderDelay FeedbackReminderDelay { get; set; }
    }
}
