//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.BapClient.Exceptions
{
    using System;
    using System.Net;
    using HR.TA.ServicePlatform.Exceptions;

    /// <summary>The bap http retriever exception.</summary>
    [Serializable]
    [MonitoredExceptionMetadata(
        HttpStatusCode.InternalServerError,
        "HR.TA.Common.BapClient",
        "BAPServiceHttp_Exception",
        MonitoredExceptionKind.Service)]

    public class BapHttpRetrieverException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BapHttpRetrieverException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BapHttpRetrieverException(string message)
            : base(message)
        {
        }
        
        public BapHttpRetrieverException(string message, string bapErrorCode, string bapErrorMessage)
            : base(message)
        {
            this.BapErrorCode = bapErrorCode;
            this.BapErrorMessage = bapErrorMessage;
        }
        
        public string BapErrorCode { get; set; }
        
        public string BapErrorMessage { get; set; }
    }
}
