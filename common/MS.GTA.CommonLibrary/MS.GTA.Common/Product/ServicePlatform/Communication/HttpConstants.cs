//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Communication
{
    internal static class HttpConstants
    {
        internal static class Headers
        {
            internal const string SessionIdHeaderName = "x-ms-session-id";
            internal const string RootActivityIdHeaderName = "x-ms-root-activity-id";
            internal const string ActivityIdHeaderName = "x-ms-activity-id";
            internal const string ActivityVectorHeaderName = "x-ms-activity-vector";
            internal const string ExecutionContextHeaderName = "x-ms-execution-context";
            internal const string ErrorPayloadHeaderName = "x-ms-error-payload";
        }
    }
}
