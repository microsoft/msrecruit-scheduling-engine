//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Utils
{
    using System;
    using System.Text;
    using CommonDataService.Common.Internal;

    /// <summary>
    /// Extensions to the Encoding class
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// Returns a base 64 encoded string
        /// </summary>
        /// <param name="encoding">The encoding</param>
        /// <param name="originalString">the string to encode</param>
        /// <returns>The encoded string</returns>
        public static string Base64Encode(this Encoding encoding, string originalString)
        {
            Contract.AssertNonEmpty(originalString, nameof(originalString));

            var bytes = encoding.GetBytes(originalString);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decodes a base64 encoded string
        /// </summary>
        /// <param name="encoding">The encoding</param>
        /// <param name="encodedString">The string to decode</param>
        /// <returns>The decoded string</returns>
        public static string Base64Decode(this Encoding encoding, string encodedString)
        {
            Contract.AssertNonEmpty(encodedString, nameof(encodedString));

            var bytes = Convert.FromBase64String(encodedString);
            return encoding.GetString(bytes);
        }
    }
}
