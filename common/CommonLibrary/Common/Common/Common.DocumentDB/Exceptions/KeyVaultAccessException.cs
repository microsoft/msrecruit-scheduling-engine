//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.DocumentDB.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// KeyVaultAccessException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Common.DocumentDB.Exceptions", "KeyVaultAccessError", MonitoredExceptionKind.Service)]
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
