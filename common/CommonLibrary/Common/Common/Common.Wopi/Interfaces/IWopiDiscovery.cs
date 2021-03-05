//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Wopi.Interfaces
{
    using System.Threading.Tasks;
    using Discovery;

    /// <summary>
    /// Describes a WOPI discovery implementation
    /// </summary>
    public interface IWopiDiscovery
    {
        /// <summary>
        /// Gets the WOPI application favicon
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>A string representing the favicon URI</returns>
        Task<string> GetApplicationFavIconUrlAsync(string wopiClientUrl, string extension, WopiActions action);

        /// <summary>
        /// Gets the viewing URL that browsers can use to render the file
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>A string representing the render URI</returns>
        Task<string> GetViewingURLTemplateAsync(string wopiClientUrl, string extension, WopiActions action);

        /// <summary>
        /// Checks if an action is valid for an extension type
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>True if the event is allowed</returns>
        Task<bool> CanDoActionAsync(string wopiClientUrl, string extension, WopiActions action);

        /// <summary>
        /// Gets the WOPI client's proof information
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <returns>The WOPI proof information for the WOPI client</returns>
        Task<WopiProofInfo> GetProofInformationAsync(string wopiClientUrl);
    }
}
