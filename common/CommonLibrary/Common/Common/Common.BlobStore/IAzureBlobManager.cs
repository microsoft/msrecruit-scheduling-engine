//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Common.Contracts;

namespace Common.BlobStore
{
    /// <summary>
    /// Azure Blob Store.
    /// </summary>
    public interface IAzureBlobManager
    {
        /// <summary>
        /// Upload file using stream 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="docType"></param>
        /// <returns>The instance of <see cref ="T:Task{string}" /> representing an asynchronous operation.</returns>
        Task<string> UploadFromStreamAsync(string name, Stream stream, CandidateAttachmentDocumentType docType);

        /// <summary>
        /// Download file from blob into stream 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The instance of <see cref ="Task{Stream}" /> representing an asynchronous operation.</returns>
        Task<Stream> DownloadFromBlobAsync(string name);

        /// <summary>
        /// Download file from blob into stream 
        /// </summary>
        /// <param name="blobName">Blob Name</param>
        ///<param name="containerName">Container Name</param>
        /// <returns>The instance of <see cref ="Task{Stream}" /> representing an asynchronous operation.</returns>
        Task<Stream> DownloadFromBlobAsync(string blobName, string containerName = null);

        /// <summary>
        /// Delete blob from container 
        /// </summary>
        /// <param name="name">Name of the blob</param>
        /// <param name="containerName">Container Name if given. Otherwise will use default container as given in settings</param>
        /// <returns>The instance of <see cref ="T:Task{bool}" /> representing an asynchronous operation.</returns>
        Task<bool> DeleteBlobAsync(string name, string containerName = null);

        /// <summary>
        /// Get list of all blobs from container 
        /// </summary>
        /// <param name="containerName">Container Name if given. Otherwise will use default container as given in settings</param>
        /// <returns>The instance of <see cref ="T:Task{List{string}}" /> representing an asynchronous operation.</returns>
        Task<List<string>> GetListOfBlobs(string containerName = null);

        /// <summary>
        /// Get all the properties of the blob
        /// </summary>
        /// <param name="name">Name of the blob</param>
        /// <param name="containerName">Container Name if given. Otherwise will use default container as given in settings</param>
        /// <returns>The instance of <see cref ="Task{BlobProperties}" /> representing an asynchronous operation.</returns>
        Task<BlobProperties> GetBlobProperties(string name, string containerName = null);        
    }
}
