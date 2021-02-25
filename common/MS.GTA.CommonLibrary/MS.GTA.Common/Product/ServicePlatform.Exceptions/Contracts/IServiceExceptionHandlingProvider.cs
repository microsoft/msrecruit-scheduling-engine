//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.IO;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Defines the contract for implementing a handler capable to understand exceptions thrown within a service.
    /// </summary>
    public interface IServiceExceptionHandlingProvider
    {
        /// <summary>
        /// Determines whether the provided <paramref name="exception"/> should be considered fatal.
        /// </summary>
        bool IsExceptionFatal(Exception exception);

        /// <summary>
        /// Gets an HTTP status code for the provided <paramref name="exception"/>.
        /// </summary>
        int GetHttpStatusCode(Exception exception);

        /// <summary>
        /// Serializes the <paramref name="exception"/> into the <paramref name="stream"/>.
        /// </summary>
        /// <param name="exception">The exception to be serialized.</param>
        /// <param name="stream">The stream to receive the serialized exception.</param>
        void Serialize(Exception exception, Stream stream);
    }
}