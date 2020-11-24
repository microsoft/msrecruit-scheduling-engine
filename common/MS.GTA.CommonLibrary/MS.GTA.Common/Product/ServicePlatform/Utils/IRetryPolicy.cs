//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// Interface for implementing retry policy.
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// Policy should return true if task should be retried/attempted again false otherwise. Also should return value for next retry delay as out parameter.
        /// </summary>
        bool ShouldRetry(Exception lastException, int attemptsExecuted, TimeSpan lastAttemptDuration, TimeSpan totalDuration, out TimeSpan nextRetryDelay);
    }
}
