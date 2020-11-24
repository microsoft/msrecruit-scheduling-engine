//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WopiHeaders.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Utils
{
    /// <summary>
    /// Request header keys based off definition at <see cref="http://wopi.readthedocs.io/projects/wopirest/en/latest/common_headers.html?highlight=X-WOPI-TimeStamp"/>
    /// </summary>
    public class WopiHeaders
    {
        /// <summary>
        /// A string representing the current proof data signed using a SHA256 (A 256 bit SHA-2-encoded [FIPS 180-2]) encryption algorithm.
        /// </summary>
        public const string WopiCurrentProof = "X-WOPI-Proof";

        /// <summary>
        /// A string representing the old proof data signed using a SHA256 (A 256 bit SHA-2-encoded [FIPS 180-2]) encryption algorithm.
        /// </summary>
        public const string WopiOldProof = "X-WOPI-ProofOld";

        /// <summary>
        /// A 64-bit integer that represents the number of 100-nanosecond intervals that have elapsed between 12:00:00 midnight, January 1, 0001, UTC and the UTC time of the request.
        /// </summary>
        public const string WopiTimeStamp = "X-WOPI-TimeStamp";

        /// <summary>
        ///  An optional header field which is an integer specifying the upper bound of 
        ///  the expected size of the file being requested. The host should use the
        ///  maximum value of a 4-byte integer if this value is not set in the request. 
        ///  If the file requested is larger than this value, the host must respond with a
        ///  412 Precondition Failed.
        /// </summary>
        public const string WopiMaxExpectedSize = "X-WOPI-MaxExpectedSize";
    }
}
