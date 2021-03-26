//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ServicePlatform.Context
{
    /// <summary>
    /// Common constants for Context class.
    /// </summary>
    internal static class ContextConstants
    {
        internal static class Activity
        {
            public const int ExponentialBackOffForApiMaxRetryCount = 5;

            public const int ExponentialBackOffForApiInitialDelayInMilliSeconds = 1000;

            public const int ExponentialBackOffForApiMaxDelayInMilliSeconds = 60000;
        }
    }
}
