//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Discovery
{
    using System.Collections.Generic;

    /// <summary>
    /// The WOPI Discovery information class
    /// </summary>
    public sealed class WopiDiscoveryInfo
    {
        /// <summary>
        /// Gets the WOPI Proof Info object
        /// </summary>
        public WopiProofInfo ProofInfo { get; internal set; }

        /// <summary>
        /// Gets the WOPI Action info objects
        /// </summary>
        public IEnumerable<WopiActionInfo> ActionInfo { get; internal set; }
    }
}