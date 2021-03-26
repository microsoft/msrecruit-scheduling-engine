//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Context;
using TA.CommonLibrary.ServicePlatform.Exceptions;

namespace TA.CommonLibrary.ServicePlatform.Security
{
    /// <summary>
    /// A certificate helper to do certificate operations.
    /// </summary>
    public sealed class CertificateManager : ICertificateManager
    {
        private readonly bool onlyFindValidCertificates = false;
        private readonly ConcurrentDictionary<string, X509Certificate2> certificateCache = new ConcurrentDictionary<string, X509Certificate2>();

        public CertificateManager()
        {
        }

        internal CertificateManager(bool onlyFindValidCertificates)
        {
            this.onlyFindValidCertificates = onlyFindValidCertificates;
        }

        /// <summary>
        /// Find certificate by thumbprint in the given store name and location.
        /// </summary>
        public async Task<X509Certificate2> FindByThumbprintAsync(string thumbprint, StoreName storeName, StoreLocation storeLocation)
        {
            return await Task.FromResult(this.FindByThumbprint(thumbprint, storeName, storeLocation));
        }

        /// <summary>
        /// Find certificate by thumbprint in the given store name and location.
        /// </summary>
        public X509Certificate2 FindByThumbprint(string thumbprint, StoreName storeName, StoreLocation storeLocation)
        {
            Contract.CheckNonEmpty(thumbprint, nameof(thumbprint));

            string cacheKey = $"{thumbprint}-{storeName}-{storeLocation}";
            return ServiceContext.Activity.Execute(
                CertificateManagerFindByThumbprintActivity.Instance,
                () =>
                {
                    return certificateCache.GetOrAdd(
                        cacheKey,
                        (string keyToAdd) =>
                        {
                            CertificateManagerTrace.Instance.TraceInformation($"Certificate with thumbprint {thumbprint} was not found in the cache. Attempting to load certificate from {storeName} {storeLocation}.");

                            var store = new X509Store(storeName, storeLocation);
                            try
                            {
                                store.Open(OpenFlags.ReadOnly);

                                var certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: onlyFindValidCertificates);
                                if (certificateCollection == null || certificateCollection.Count == 0)
                                {
                                    throw new CertificateNotFoundException(thumbprint, storeName, storeLocation);
                                }

                                if (certificateCollection.Count > 1)
                                {
                                    throw new MultipleCertificatesMatchedException(thumbprint, storeName, storeLocation);
                                }

                                var pfxCert = certificateCollection[0];
                                CertificateManagerTrace.Instance.TraceVerbose($"Found certifcate {pfxCert.Thumbprint}. Has private key: {pfxCert.HasPrivateKey}.");

                                return pfxCert;
                            }
                            finally
                            {
                                store.Close();
                            }
                        });
                });
        }
    }
}
