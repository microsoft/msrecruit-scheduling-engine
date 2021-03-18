//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Discovery
{
    /// <summary>
    /// Supported WOPI actions <see cref="http://wopi.readthedocs.io/en/latest/discovery.html#wopi-actions"/>
    /// </summary>
    public enum WopiActions
    {
        /// <summary>
        /// An action that renders a non-editable view of a document that is optimized for embedding in a web page.
        /// </summary>
        EmbedView,

        /// <summary>
        /// An action that provides a static image preview of the file type.
        /// </summary>
        ImagePreview,

        /// <summary>
        /// An action that provides an interactive preview of the file type.
        /// </summary>
        InteractivePreview,

        /// <summary>
        /// An action used to preload static content for Office Online view applications.
        /// </summary>
        PreloadView,

        /// <summary>
        /// An action that renders a non-editable view of a document.
        /// </summary>
        View,
    }
}
