//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EmailContent.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using MS.GTA.Talent.EnumSetModel.SchedulingService;
    using System.Runtime.Serialization;

    /// <summary>
    /// All required email content with order
    /// </summary>
    [DataContract]
    public class EmailContent
    {
        /// <summary>
        /// Gets or sets Actual email content
        /// </summary>
        [DataMember(Name = "content", IsRequired = false)]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets Order for Section
        /// </summary>
        [DataMember(Name = "order", IsRequired = false)]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets Email Section - where to fit this content
        /// </summary>
        [DataMember(Name = "emailContentType", IsRequired = false)]
        public EmailContentType EmailContentType { get; set; }
    }
}
