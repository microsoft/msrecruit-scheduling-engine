//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Exceptions;

namespace HR.TA.ServicePlatform.AspNetCore.Builder
{
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class WebApiHostAlreadyRegisteredException : MonitoredException
    {
        internal WebApiHostAlreadyRegisteredException(object registeredInstance)
            : base("A web API host is already registered")
        {
            Contract.AssertValue(registeredInstance, nameof(registeredInstance));
            RegisteredInstance = registeredInstance;
        }

        [ExceptionCustomData]
        public object RegisteredInstance { get; private set; }
    }
}
