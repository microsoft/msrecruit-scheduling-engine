//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using CommonDataService.Common.Internal;
using ServicePlatform.Communication.Http.Routers;
using ServicePlatform.Exceptions;

namespace ServicePlatform.Communication.Http
{
    /// <summary>
    /// Thrown when router fails to stop the retry loop.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class RoutingHandlerRetryLimitReachedException : MonitoredException
    {
        internal RoutingHandlerRetryLimitReachedException(IHttpRouter router)
        {
            Contract.AssertValue(router, nameof(router));
            Router = router;
        }

        public IHttpRouter Router { get; }

        [ExceptionCustomData]
        public string RouterName
        {
            get { return Router.Name; }
        }
    }
}
