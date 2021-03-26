//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using HR.TA.ServicePlatform.Exceptions;

namespace HR.TA.ServicePlatform.Flighting
{
    /// <summary>
    /// Thrown when the configured flights provider is missing from the service dependency container.
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class FlightsProviderMissingException : MonitoredException
    {
        internal FlightsProviderMissingException(Type flightsProviderType)
            : base($"There is no flights provider registered for '{flightsProviderType?.FullName}'")
        {
        }
    }
}
