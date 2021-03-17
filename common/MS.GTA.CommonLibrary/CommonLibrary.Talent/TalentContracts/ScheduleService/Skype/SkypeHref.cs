//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Skype for business hyperlink
    /// </summary>
    [DataContract]
    public class SkypeHref
    {
        /// <summary>
        /// Gets or sets the Skype for business hyperlink
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
