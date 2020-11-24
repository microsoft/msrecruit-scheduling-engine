// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalApiException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Communication.Http.Exceptions
{
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// ExternalApi Task cancelled exceeption
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "ExternalDependencyFailure", "ExternalApiTaskCancelledFailure", MonitoredExceptionKind.Benign)]
    public sealed class ExternalApiTaskCancelledException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalApiTaskCancelledException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ExternalApiTaskCancelledException(string message)
            : base(message)
        {

        }
    }
}
