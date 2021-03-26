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
    /// KeyVaultAccessException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "HR.TA.Common.DocumentDB.Exceptions", "KeyVaultAccessError", MonitoredExceptionKind.Service)]
    public class KeyVaultAccessException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultAccessException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public KeyVaultAccessException(string message)
            : base(message)
        {
        }
    }
}
