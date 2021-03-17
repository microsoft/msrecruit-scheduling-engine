//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.DocumentDB.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// DocumentDBException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "CommonLibrary.Common.DocumentDB.Exceptions", "KeyVaultAccessError", MonitoredExceptionKind.Service)]
    public class DocumentDBException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDBException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DocumentDBException(string message)
            : base(message)
        {
        }
    }
}
