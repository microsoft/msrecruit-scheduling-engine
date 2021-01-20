//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MS.GTA.ServicePlatform.Security
{
    /// <summary>
    /// A certificate helper to do certificate operations.
    /// </summary>
    public interface ICertificateManager
    {
        /// <summary>
        /// Find certificate by thumbprint.
        /// </summary>
        X509Certificate2 FindByThumbprint(string thumbprint, StoreName storeName, StoreLocation storeLocation);

        /// <summary>
        /// Find certificate by thumbprint asynchronously.
        /// </summary>
        Task<X509Certificate2> FindByThumbprintAsync(string thumbprint, StoreName storeName, StoreLocation storeLocation);
    }
}
