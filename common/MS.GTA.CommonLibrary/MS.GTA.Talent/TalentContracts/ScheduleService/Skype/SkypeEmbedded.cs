// <copyright file="SkypeEmbedded.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Skype for business embedded
    /// </summary>
    [DataContract]
    public class SkypeEmbedded
    {
        /// <summary>
        /// Gets or sets the Skype online meeting extension
        /// </summary>
        [DataMember(Name = "onlineMeetingExtension")]
        public SkypeOnlineMeetingExtension[] OnlineMeetingExtension { get; set; }
    }
}
