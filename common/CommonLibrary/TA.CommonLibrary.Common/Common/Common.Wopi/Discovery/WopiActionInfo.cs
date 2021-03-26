//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Wopi.Discovery
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
