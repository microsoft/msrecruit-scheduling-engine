//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Wopi.Interfaces
{
    using System.Threading.Tasks;
    using Utils;

    /// <summary>
    /// Represents a store that allows IWOPI files to be retrieved
    /// </summary>
    public interface IWopiStore
    {
        /// <summary>
        /// Returns a file instance implementing the <see cref="IWopiFile"/> interface
        /// </summary>
        /// <param name="fileId">The fileId, must be unique for a file</param>
        /// <param name="tokenInfo">The token Info object</param>
        /// <returns>The retrieved file</returns>
        Task<IWopiFile> GetWopiFile(string fileId, TokenInfo tokenInfo);
    }
}
