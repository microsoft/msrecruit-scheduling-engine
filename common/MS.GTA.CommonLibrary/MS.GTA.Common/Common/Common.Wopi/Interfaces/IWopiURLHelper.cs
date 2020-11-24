//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IWopiURLHelper.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Interfaces
{
    using System.Threading.Tasks;
    using Discovery;

    /// <summary>
    /// The WOPI URL helper interface
    /// </summary>
    public interface IWopiURLHelper
    {
        /// <summary>
        /// Gets the full viewing URL to be sent to a browser for viewing a file
        /// </summary>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The desired action to carry out on the extension type</param>
        /// <param name="fileId">The file to carry out the action on</param>
        /// <returns>The URL to be sent to browser clients for carrying out the action on the file</returns>
        Task<string> GetViewingURLAsync(
            string extension,
            WopiActions action,
            string fileId);
    }
}
