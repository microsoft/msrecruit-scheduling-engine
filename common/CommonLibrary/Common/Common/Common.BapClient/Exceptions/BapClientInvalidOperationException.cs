//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient.Exceptions
{
    using Common.Base;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Privacy;
    using System;
    using System.Net;

    /// <summary>
    /// Invalid BAP Client operation exception
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "Common.BapClient.Exceptions", nameof(BapClientInvalidOperationException), MonitoredExceptionKind.Benign)]
    public sealed class BapClientInvalidOperationException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BapClientInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BapClientInvalidOperationException(string message)
            : base(message)
        {
            ErrorMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BapClientInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BapClientInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorMessage = message;
        }

        [ExceptionCustomData(Serialize = true, Name = Constants.ExceptionMessageTypeName, PrivacyLevel = PrivacyLevel.CustomerData)]
        public string ErrorMessage { get; private set; }

        [ExceptionCustomData(Serialize = true, Name = Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData)]
        public int ErrorType => (int)Base.ErrorType.Fatal;
    }
}
