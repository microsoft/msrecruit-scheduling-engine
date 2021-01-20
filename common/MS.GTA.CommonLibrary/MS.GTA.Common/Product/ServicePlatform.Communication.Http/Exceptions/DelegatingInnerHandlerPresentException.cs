//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Thrown when a custom delegating handler already has an assigned InnerHandler.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class DelegatingInnerHandlerPresentException : MonitoredException
    {
        internal DelegatingInnerHandlerPresentException(DelegatingHandler delegatingHandler)
            : base("Custom delegating handler already has an InnerHandler assigned")
        {
            Contract.AssertValue(delegatingHandler, nameof(delegatingHandler));

            DelegatingHandler = delegatingHandler;
        }

        /// <summary>
        /// Gets the offending delegating handler.
        /// </summary>
        [ExceptionCustomData]
        public DelegatingHandler DelegatingHandler { get; }
    }
}
