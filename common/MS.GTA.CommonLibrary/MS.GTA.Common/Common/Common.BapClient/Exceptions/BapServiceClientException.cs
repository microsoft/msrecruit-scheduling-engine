//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.BapClient.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Privacy;

    /// <summary>
    /// BAP service client exceptions
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.BapClient", "BAPService_Exception", MonitoredExceptionKind.Service)]
    public class BapServiceClientException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BapServiceClientException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BapServiceClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BapServiceClientException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BapServiceClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BapServiceClientException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="statusCode">The HTTP status code of the failed request.</param>
        /// <param name="responseHeaders">The response headers.</param>
        /// <param name="innerException">The inner exception.</param>
        public BapServiceClientException(string message, HttpStatusCode statusCode, IDictionary<string, string> responseHeaders, Exception innerException)
            : base(message, innerException)
        {
            this.StatusCode = statusCode;
            this.ResponseHeaders = responseHeaders;
        }

        /// <summary>
        /// Gets or sets HTTP Status code of the request.
        /// </summary>
        [ExceptionCustomData(
        Name = "StatusCode",
        Serialize = true,
        PrivacyLevel = PrivacyLevel.PublicData)]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the response headers for the request that raised the exception.
        /// </summary>
        [ExceptionCustomData(
        Name = "ResponseHeaders",
        Serialize = true,
        PrivacyLevel = PrivacyLevel.PublicData)]
        public IDictionary<string, string> ResponseHeaders { get; set; }

        /// <summary>
        /// GetObjectData calls into GetObjectData of Serializable
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.StatusCode), this.StatusCode);
            info.AddValue(nameof(this.ResponseHeaders), this.ResponseHeaders);
        }
    }
}
