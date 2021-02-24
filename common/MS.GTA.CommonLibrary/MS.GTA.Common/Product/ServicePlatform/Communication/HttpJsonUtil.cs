//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Utils;
using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.Communication
{
    /// <summary>
    /// This class contains Json Serializer/Deserializer
    /// For Adding RootExecutionContext to Http Request Header
    /// and Getting RootExecutionContext from Http Request Header 
    /// </summary>
    internal static class HttpJsonUtil
    {
        /// <summary>
        /// 1. Serialize it as JSON string
        /// 2. Transform JSON string to Bytes in UTF-8 
        /// 3. Encode it to Base64String
        /// </summary>
        /// <param name="rootExecutionContext">The root execution context object.</param>
        /// <returns>Base64String</returns>
        internal static string JsonSerialize(RootExecutionContext rootExecutionContext)
        {
            var jsonStringContext = JsonConvert.SerializeObject(rootExecutionContext);
            return Convert.ToBase64String(EncodingUtil.UTF8.GetBytes(jsonStringContext));
        }

        internal static RootExecutionContext JsonDeserialize(JsonSerializer jsonSerializer, string encodedContext)
        {
            RootExecutionContext executionContext = null;

            if (encodedContext != null)
            {
                // In case the context header is malformed this will throw and the error 
                // will be propagated back to the caller
                //
                var contextBytes = Convert.FromBase64String(encodedContext);
                var contextString = EncodingUtil.UTF8.GetString(contextBytes);
                using (var stringReader = new StringReader(contextString))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    executionContext = jsonSerializer.Deserialize<RootExecutionContext>(jsonReader);
                }
            }

            return executionContext;
        }
    }
}
