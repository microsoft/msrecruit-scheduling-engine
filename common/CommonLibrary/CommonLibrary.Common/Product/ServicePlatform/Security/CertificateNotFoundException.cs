//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using CommonLibrary.ServicePlatform.Exceptions;

namespace CommonLibrary.ServicePlatform.Security
{
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class CertificateNotFoundException : MonitoredException
    {
        public CertificateNotFoundException(string thumbprint, StoreName storeName, StoreLocation storeLocation)
            : base($"Could not find certificate with thumbprint {thumbprint}. Certificate store name: {storeName} location: {storeLocation}")
        {
        }
    }

    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class CertificateNotFoundInKeyVaultException : MonitoredException
    {
        public CertificateNotFoundInKeyVaultException(string certificateName)
            : base($"Could not find certificate {certificateName} in key vault. This certificate likely does not exist.")
        {
        }
    }
}
