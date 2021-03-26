//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ServicePlatform.Fabric
{
    /// <summary>
    /// Service Fabric well-known constants.
    /// </summary>
    internal static class FabricConstants
    {
        /// <summary>
        /// Service Fabric well-known HTTP constants.
        /// </summary>
        internal static class Http
        {
            /// <summary>
            /// HTTP header name for Service Fabric request/response metadata.
            /// </summary>
            public const string ServiceFabricHeaderName = "X-ServiceFabric";

            /// <summary>
            /// HTTP header value for Service Fabric resolution retry bypass on HTTP 404 - Not Found
            /// </summary>
            public const string ResourceNotFoundHeaderValue = "ResourceNotFound";
        }
    }
}
