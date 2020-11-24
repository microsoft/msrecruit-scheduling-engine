//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication.Http.Routers;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
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
