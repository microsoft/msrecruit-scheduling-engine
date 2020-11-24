//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ScheduleEmail.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEngagementService.Data;

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
