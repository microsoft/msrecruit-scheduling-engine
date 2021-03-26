//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using System.Net.Http;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Exceptions;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http
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
