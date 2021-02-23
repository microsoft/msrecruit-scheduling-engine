//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// An outlook calendar event
    /// </summary>
    [DataContract]
    public class FreeBusyError
    {
        /// <summary>
        /// Gets or sets error message
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets resposne code
        /// </summary>
        [DataMember(Name = "responseCode")]
        public string ResponseCode { get; set; }
    }
}
