//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using System.Net.Http;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Exceptions;

namespace HR.TA.ServicePlatform.Communication.Http
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
