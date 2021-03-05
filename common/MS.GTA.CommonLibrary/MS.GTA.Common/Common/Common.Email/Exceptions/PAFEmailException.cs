//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Email.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// GraphEmailException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "Common.Email.Exceptions", "PAFEmailException", MonitoredExceptionKind.Service)]
    public class PAFEmailException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PAFEmailException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public PAFEmailException(string message)
            : base(message)
        {
        }
    }
}
