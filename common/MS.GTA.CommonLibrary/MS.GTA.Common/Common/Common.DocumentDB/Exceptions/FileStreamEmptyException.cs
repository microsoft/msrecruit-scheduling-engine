//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="FileStreamEmptyException.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB.Exceptions
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// FileStreamEmptyException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.Common.DocumentDB.Exceptions", "EmptyFile", MonitoredExceptionKind.Service)]
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