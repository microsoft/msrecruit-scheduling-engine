﻿//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Security
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
