//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="DocumentDBException.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// DocumentDBException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.DocumentDB.Exceptions", "KeyVaultAccessError", MonitoredExceptionKind.Service)]
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