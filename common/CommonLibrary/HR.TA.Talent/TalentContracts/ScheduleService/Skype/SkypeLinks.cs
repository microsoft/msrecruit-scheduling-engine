//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Skype for business links
    /// </summary>
    [DataContract]
    public class SkypeLinks
    {
        /// <summary>
        /// Gets or sets the Skype for business self link
        /// </summary>
        [DataMember(Name = "self")]
        public SkypeHref Self { get; set; }

        /// <summary>
        /// Gets or sets the Skype for business text view link
        /// </summary>
        [DataMember(Name = "textView")]
        public SkypeHref TextView { get; set; }

        /// <summary>
        /// Gets or sets the Skype for business html view link
        /// </summary>
        [DataMember(Name = "htmlView")]
        public SkypeHref HtmlView { get; set; }
    }
}
