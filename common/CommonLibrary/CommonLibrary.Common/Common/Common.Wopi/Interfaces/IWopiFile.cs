//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes a WOPI file
    /// </summary>
    public interface IWopiFile
    {
        /// <summary>
        /// Gets the file name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the unique id needed to retrieve the file
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the size of the file 
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Gets the file's extension
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets the file owner
        /// </summary>
        string OwnerId { get; }

        /// <summary>
        /// Gets the file's version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the file contents as a stream
        /// </summary>
        /// <returns>The file contents as a stream task</returns>
        Task<Stream> GetContents();
    }
}
