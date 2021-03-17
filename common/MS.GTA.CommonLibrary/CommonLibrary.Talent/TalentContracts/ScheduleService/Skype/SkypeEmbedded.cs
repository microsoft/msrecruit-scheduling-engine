//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
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
