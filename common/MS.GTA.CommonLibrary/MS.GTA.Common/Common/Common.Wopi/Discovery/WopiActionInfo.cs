//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WopiActionInfo.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Discovery
{
    /// <summary>
    /// The WOPI Action information class
    /// </summary>
    public sealed class WopiActionInfo
    {
        /// <summary>
        /// Gets the FAVIcon url
        /// </summary>
        public string FavIconUrl { get; internal set; }

        /// <summary>
        /// Gets the File Extension
        /// </summary>
        public string Extension { get; internal set; }

        /// <summary>
        /// Gets the file Rendering Url
        /// </summary>
        public string Url { get; internal set; }

        /// <summary>
        /// Gets the file WOPI Action name
        /// </summary>
        public string Action { get; internal set; }
    }
}
