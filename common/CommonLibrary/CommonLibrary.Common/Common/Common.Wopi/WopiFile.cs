//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces;

    /// <summary>
    /// Implementation of the WOPIFile interface
    /// </summary>
    public class WopiFile : IWopiFile
    {
        /// <summary>
        /// The FileInfo object representing the file
        /// </summary>
        private FileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiFile"/> class.
        /// </summary>
        /// <param name="fileId">The file id</param>
        /// <param name="filePath">The path for retrieving the file</param>
        public WopiFile(string fileId, string filePath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileId), "File id must be set");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(filePath), "File path must be set");

            this.FilePath = filePath;
            this.Id = fileId;
        }

        /// <summary>
        /// Gets the file's extension
        /// </summary>
        public string Extension
        {
            get
            {
                var extension = this.FileInfo.Extension;
                if (extension.StartsWith("."))
                {
                    extension = extension.Substring(1);
                }

                return extension;
            }
        }

        /// <summary>
        /// Gets the file id needed to retrieve the file
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        public string Name
        {
            get
            {
                return this.FileInfo.Name;
            }
        }

        /// <summary>
        /// Gets or sets the file owner
        /// </summary>
        public string OwnerId
        {
            get; set;
        }

        /// <summary>
        /// Gets the size of the file 
        /// </summary>
        public long Size
        {
            get
            {
                return this.FileInfo.Length;
            }
        }

        /// <summary>
        /// Gets the file's version
        /// </summary>
        public string Version
        {
            get
            {
                return this.FileInfo.LastWriteTimeUtc.ToLongDateString();
            }
        }

        /// <summary>
        /// Gets the file path
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the FileInfo object containing information about the file
        /// </summary>
        private FileInfo FileInfo
        {
            get
            {
                if (this.fileInfo == null)
                {
                    this.fileInfo = new FileInfo(this.FilePath);
                }

                return this.fileInfo;
            }
        }

        /// <summary>
        /// Gets the file contents as a stream
        /// </summary>
        /// <returns>The file contents as a stream</returns>
        public Task<Stream> GetContents()
        {
            Stream fileStream = this.FileInfo.OpenRead();
            return Task.FromResult(fileStream);
        }
    }
}
