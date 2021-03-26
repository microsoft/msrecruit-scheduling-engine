//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Text;

namespace TA.CommonLibrary.ServicePlatform.Utils
{
    public static class EncodingUtil
    {
        /// <summary>
        /// A secure and explicit version of the UTF8 Encoding. Services should use this instead of 
        /// the default <see cref="Encoding.UTF8"/> version.
        /// </summary>
        public static Encoding UTF8 { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    }
}
