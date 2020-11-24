// <copyright file="SkypeHref.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
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
