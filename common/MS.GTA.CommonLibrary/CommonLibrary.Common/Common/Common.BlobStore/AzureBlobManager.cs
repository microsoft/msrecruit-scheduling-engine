//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using CommonLibrary.Common.Base.KeyVault;
using CommonLibrary.Common.BlobStore.Configuration;
using CommonLibrary.Common.Contracts;
using CommonLibrary.CommonDataService.Common;
using CommonLibrary.ServicePlatform.Configuration;
using CommonLibrary.ServicePlatform.Tracing;

namespace CommonLibrary.Common.BlobStore
{
    /// <summary>
    /// Blob Storage
    /// </summary>
    public class AzureBlobManager : IAzureBlobManager
    {
        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Secret manager provider
        /// </summary>
        private readonly ISecretManagerProvider secretManagerProvider;

        /// <summary>
        /// Blob store Settings
        /// </summary>
        private readonly BlobStoreSettings blobStoreSettings;

        private CloudBlobClient cloudBlobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobManager" /> class.
        /// </summary>
        /// <param name="trace">Trace source instance</param>
        /// <param name="configManager">The configuration manager instance to use.</param>
        /// <param name="secretManagerProvider">The Secret Manager Provider.</param>
        public AzureBlobManager(ITraceSource trace, IConfigurationManager configManager, ISecretManagerProvider secretManagerProvider)
        {
            this.trace = trace;
            this.blobStoreSettings = configManager.Get<BlobStoreSettings>();            
            this.secretManagerProvider = secretManagerProvider;
            GetBlobClient().GetAwaiter().GetResult();
        }


        /// <summary>
        /// Upload file using stream 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="docType">The type of the document.</param>
        /// <returns></returns>
        public async Task<string> UploadFromStreamAsync(string name, Stream stream, CandidateAttachmentDocumentType docType)
        {
            Contract.CheckNonEmpty(name, nameof(name));
            try
            {
                CloudBlobContainer container = await this.GetBlobContainerAsync();

                var blockBlob = container.GetBlockBlobReference(name);
                Tuple<CandidateAttachmentDocumentType, string>[] SupportedDocTypes = new Tuple<CandidateAttachmentDocumentType, string>[]
                {
                    Tuple.Create(CandidateAttachmentDocumentType.AVI, "video/x-msvideo"),
                    Tuple.Create(CandidateAttachmentDocumentType.DOC, "application/msword"),
                    Tuple.Create(CandidateAttachmentDocumentType.DOCX, "application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
                    Tuple.Create(CandidateAttachmentDocumentType.HTML, "text/html"),
                    Tuple.Create(CandidateAttachmentDocumentType.JPG, "image/jpeg"),
                    Tuple.Create(CandidateAttachmentDocumentType.MP4, "video/mp4"),
                    Tuple.Create(CandidateAttachmentDocumentType.ODT, "application/vnd.oasis.opendocument.text"),
                    Tuple.Create(CandidateAttachmentDocumentType.PDF, "application/pdf"),
                    Tuple.Create(CandidateAttachmentDocumentType.PPTX, "application/vnd.openxmlformats-officedocument.presentationml.presentation"),
                    Tuple.Create(CandidateAttachmentDocumentType.RTF, "text/richtext"),
                    Tuple.Create(CandidateAttachmentDocumentType.TXT, "text/plain")
                };
                IDictionary<CandidateAttachmentDocumentType, string> DocTypeExtensionMap = SupportedDocTypes.ToDictionary(s => s.Item1, s => s.Item2);
                if (DocTypeExtensionMap.ContainsKey(docType))
                {
                    blockBlob.Properties.ContentType = DocTypeExtensionMap[docType];
                }
                else
                {
                    blockBlob.Properties.ContentType = "application/octet-stream";
                }
                using (var fileStream = stream)
                {
                    this.trace.TraceInformation($"Uploading the blob {name} into container {container.Name}");
                    await blockBlob.UploadFromStreamAsync(fileStream);
                    string url = blockBlob.Uri.ToString();
                    return url;
                }
            }
            catch (StorageException)
            {
                trace.TraceError($"UploadFromStreamAsync: Failed to upload {name} file in blob ");
                throw;
            }
        }

        /// <summary>
        /// Download file from blob into stream 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Stream> DownloadFromBlobAsync(string name)
        {
            Contract.CheckNonEmpty(name, nameof(name));
            try
            {
                CloudBlobContainer container = await this.GetBlobContainerAsync();

                var blockBlob = container.GetBlockBlobReference(name);
                MemoryStream ms = null;
                if (await blockBlob.ExistsAsync())
                {
                    ms = new MemoryStream();
                    this.trace.TraceInformation($"Downloading the blob {name} from container {container.Name}");
                    await blockBlob.DownloadToStreamAsync(ms);
                }
                this.trace.TraceInformation($"Blob: {name} from container {container.Name} does not exist");
                return ms;
            }
            catch (StorageException e)
            {
                trace.TraceError($"DownloadFromBlobAsync: Failed to download {name} file in blob with Error;{e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Download file from blob into stream 
        /// </summary>
        /// <param name="blobName">Blob Name</param>
        ///<param name="containerName">Container Name</param>
        /// <returns></returns>
        public async Task<Stream> DownloadFromBlobAsync(string blobName, string containerName = null)
        {
            Contract.CheckNonEmpty(blobName, nameof(blobName));
            

            try
            {
                CloudBlobContainer container = await this.GetBlobContainerAsync(containerName);

                var blockBlob = container.GetBlockBlobReference(blobName);
                MemoryStream memoryStream = null;
                if (await blockBlob.ExistsAsync())
                {
                    memoryStream = new MemoryStream();
                    this.trace.TraceInformation($"Downloading the blob {blobName} from container {container.Name}");
                    await blockBlob.DownloadToStreamAsync(memoryStream);
                }
                this.trace.TraceInformation($"Blob: {blobName} from container {container.Name} does not exist");
                return memoryStream;
            }
            catch (StorageException)
            {
                trace.TraceError($"DownloadFromBlobAsync: Failed to download {blobName} file in blob ");
                throw;
            }
        }

        /// <summary>
        /// Delete blob from container 
        /// </summary>
        /// <param name="name">Name of the blob</param>
        /// <param name="containerName">Blob Container</param>
        /// <returns></returns>
        public async Task<bool> DeleteBlobAsync(string name, string containerName = null)
        {
            Contract.CheckNonEmpty(name, nameof(name));
            CloudBlobContainer container = null;
            try
            {
                container = await this.GetBlobContainerAsync(containerName);                    
                var blockBlob = container.GetBlockBlobReference(name);
                return await blockBlob.DeleteIfExistsAsync();
            }
            catch (StorageException)
            {
                trace.TraceError($"DeleteBlobAsync: Failed to delete blob: {name} in container:{container?.Name} ");
                throw;
            }
        }

        /// <summary>
        /// Get list of all blobs from container 
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetListOfBlobs(string containerName = null)
        {
            try
            {
                List<string> blobNames = new List<string>();
                CloudBlobContainer container = await this.GetBlobContainerAsync(containerName);
                BlobContinuationToken blobContinuationToken = null;
                do
                {
                    var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
                    // Get the value of the continuation token returned by the listing call.
                    blobContinuationToken = results.ContinuationToken;

                    foreach (CloudBlockBlob item in results.Results)
                    {
                        blobNames.Add(item.Name);
                    }
                } while (blobContinuationToken != null); // Loop while the continuation token is not null.
                return blobNames;
            }
            catch (StorageException)
            {
                trace.TraceWarning($"GetListOfBlobs: Failed to fetch list of blobs ");
                throw;
            }
        }

        /// <summary>
        /// Get all the properties of the blob
        /// </summary>
        /// <returns></returns>
        public async Task<BlobProperties> GetBlobProperties(string name, string containerName = null)
        {
            try
            {
                CloudBlobContainer container = await this.GetBlobContainerAsync(containerName);

                var blockBlob = container.GetBlockBlobReference(name);
                await blockBlob.FetchAttributesAsync();
                return blockBlob.Properties;
            }
            catch (StorageException)
            {
                trace.TraceWarning($"GetBlobProperties: Failed to fetch blob properties.");
                throw;
            }
        }

        /// <summary>
        /// Gets the specified Blob Container
        /// </summary>
        /// <param name="containerName">Container Name if given. Otherwise Default container as given in settings</param>
        /// <returns></returns>
        public async Task<CloudBlobContainer> GetBlobContainerAsync(string containerName = null)
        {
            if (cloudBlobClient == null)
                await GetBlobClient();

            CloudBlobContainer container = string.IsNullOrWhiteSpace(containerName)
                ? cloudBlobClient.GetContainerReference(this.blobStoreSettings.BlobStoreContainerName)
                : cloudBlobClient.GetContainerReference(containerName);
                
            await container.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            };
            await container.SetPermissionsAsync(permissions);

            return container;
        }       

        private async Task GetBlobClient()
        {
            if(this.blobStoreSettings == null)
            {
                throw new InvalidDataContractException($"The Azure Blob Storage Settings cannot be null");
            }

            var keyVaultUri = this.blobStoreSettings.BlobKeyVaultUri;
            var secretManager = this.secretManagerProvider.GetOrCreateSecretManager(keyVaultUri);
            var connectionStringSecret = await secretManager.TryGetSecretAsync(this.blobStoreSettings.KeyVaultSecretNameForPrimaryConnectionString);
            if (!connectionStringSecret.Succeeded)
            {
                throw new InvalidOperationException($"GetBlobContainer: Connection String of Blob Storage: {this.blobStoreSettings.KeyVaultSecretNameForPrimaryConnectionString}, not set in Key Vault:{keyVaultUri}");                
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionStringSecret.Result.Value);

            //Client  
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }
    }
}

