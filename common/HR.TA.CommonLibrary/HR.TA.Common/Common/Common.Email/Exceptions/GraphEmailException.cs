//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Email.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// GraphEmailException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "Microsoft.D365.HCM.Common.Email.Exceptions", "GraphEmailException", MonitoredExceptionKind.Service)]
    public class GraphEmailException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEmailException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GraphEmailException(string message)
            : base(message)
        {
        }
    }
}
