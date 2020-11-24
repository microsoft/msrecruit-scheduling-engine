//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// GraphEmailException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.Common.Email.Exceptions", "PAFEmailException", MonitoredExceptionKind.Service)]
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
