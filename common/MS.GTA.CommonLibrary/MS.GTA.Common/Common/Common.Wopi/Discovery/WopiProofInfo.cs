//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WopiProofInfo.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Discovery
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
