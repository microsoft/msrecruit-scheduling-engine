//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Discovery
{
    /// <summary>
    /// The WOPI Proof Information class
    /// </summary>
    public sealed class WopiProofInfo
    {
        /// <summary>
        /// Gets the old proof key from WOPI <see cref="http://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html#troubleshooting-proof-keys"/>
        /// </summary>
        public string OldCspBlob { get; internal set; }

        /// <summary>
        /// Gets the new proof key from WOPI <see cref="http://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html#troubleshooting-proof-keys"/>
        /// </summary>
        public string CurrentCspBlob { get; internal set; }
    }
}
