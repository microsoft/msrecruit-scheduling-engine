//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEngagementService.Data;

    /// <summary>
    /// The candidate availability schedule email data contract.
    /// </summary>
    [DataContract]
    public class ScheduleEmail
    {
        /// <summary>
        /// Gets or sets email subject.
        /// </summary>
        [DataMember(Name = "subject", IsRequired = false)]
        public string MessageSubject { get; set; }

        /// <summary>
        /// Gets or sets paragraph one.
        /// </summary>
        [DataMember(Name = "paragraph1", IsRequired = false)]
        public string Paragraph1 { get; set; }
    }
}
