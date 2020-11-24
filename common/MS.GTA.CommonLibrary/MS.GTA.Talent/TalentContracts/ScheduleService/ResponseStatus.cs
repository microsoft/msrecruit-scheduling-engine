// <copyright file="ResponseStatus.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The outlook calendar event response status
    /// </summary>
    [DataContract]
    public class ResponseStatus
    {
        /// <summary>
        /// Gets or sets the event response
        /// </summary>
        [DataMember(Name = "response")]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the event time
        /// </summary>
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }
    }
}
