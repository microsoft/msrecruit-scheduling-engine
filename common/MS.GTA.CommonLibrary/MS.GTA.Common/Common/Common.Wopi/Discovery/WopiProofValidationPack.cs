//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Wopi.Discovery
{
    /// <summary>
    /// WOPI proof validation pack class <see cref="http://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html#troubleshooting-proof-keys"/>
    /// </summary>
    public sealed class WopiProofValidationPack
    {
        /// <summary>
        /// Gets or sets the current key from the WOPI client discovery XML
        /// </summary>
        public string CurrentDiscoveryKey { get; set; }

        /// <summary>
        /// Gets or sets the old key from the WOPI client discovery XML
        /// </summary>
        public string OldDiscoveryKey { get; set; }

        /// <summary>
        /// Gets or sets the AccessToken from the WOPI request
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///  Gets or sets the WOPI request URL in all uppercase; includes all query string parameters.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///  Gets or sets the value of the timestamp from the X-WOPI-TimeStamp header value
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        ///  Gets or sets the value of the current proof from the request header
        /// </summary>
        public string CurrentHeaderProof { get; set; }

        /// <summary>
        ///  Gets or sets the value of the old proof from the request header
        /// </summary>
        public string OldHeaderProof { get; set; }
    }
}
