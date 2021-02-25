//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Net;
using System.Security.Cryptography.X509Certificates;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Security
{
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class MultipleCertificatesMatchedException : MonitoredException
    {
        public MultipleCertificatesMatchedException(string thumbprint, StoreName storeName, StoreLocation storeLocation)
            : base($"Multiple certificates with same thumbprint: {thumbprint} were found. Certificate store name: {storeName} location: {storeLocation}")
        {
        }
    }
}
