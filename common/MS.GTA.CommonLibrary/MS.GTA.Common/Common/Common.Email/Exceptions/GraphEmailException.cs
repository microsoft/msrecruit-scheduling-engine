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
