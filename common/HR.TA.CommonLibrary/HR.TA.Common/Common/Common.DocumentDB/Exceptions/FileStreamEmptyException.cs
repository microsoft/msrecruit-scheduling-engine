//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.DocumentDB.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// FileStreamEmptyException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "HR.TA.Common.DocumentDB.Exceptions", "EmptyFile", MonitoredExceptionKind.Service)]
    public class FileStreamEmptyException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamEmptyException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public FileStreamEmptyException(string message)
            : base(message)
        {
        }
    }
}
