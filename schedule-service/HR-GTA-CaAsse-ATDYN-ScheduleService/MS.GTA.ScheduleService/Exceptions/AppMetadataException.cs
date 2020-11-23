//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AppMetadataException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// AppMetadataException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.InterviewService.Exceptions", "Plugins", MonitoredExceptionKind.Benign)]
    public class AppMetadataException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppMetadataException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AppMetadataException(string message)
            : base(message)
        {
            this.ErrorMessage = message;
        }

        /// <summary>
        /// Gets the error message to be passed to the client.
        /// </summary>
        [ExceptionCustomData(Name = "errorMessage", Serialize = true)]
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// GetObjectData calls into GetObjectData of Serializable
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.ErrorMessage), this.ErrorMessage);
        }
    }
}