//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// Collection of methods to executes tasks according to given retry policy.
    /// </summary>
    public static class RetryUtil
    {
        /// <summary>
        /// Executes asynchronous function using given retry policy.
        /// </summary>
        public static async Task<T> ExecuteAsync<T>(
            this IRetryPolicy retryPolicy,
            Func<Task<T>> func)
        {
            Contract.CheckValue(func, nameof(func));
            Contract.CheckValue(retryPolicy, nameof(retryPolicy));

            var attemptNumber = 1;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            { 
                var retryStopwatch = new Stopwatch();

                try
                {
                    return await func();
                }
                catch (Exception ex) when (!ex.IsFatal())
                {
                    var retryInterval = TimeSpan.Zero;
                    var shouldRetry = retryPolicy.ShouldRetry(ex, attemptNumber, retryStopwatch.Elapsed, stopwatch.Elapsed, out retryInterval);

                    if (!shouldRetry)
                    {
                        UtilTrace.Instance.TraceError($"All retry attempts failed.");
                        throw;
                    }

                    UtilTrace.Instance.TraceWarning($"Retry attempt failed: {attemptNumber}, exception: {ex}");
                    await Task.Delay(retryInterval);
                    attemptNumber++;
                }
            }
        }

        /// <summary>
        /// Executes asynchronous function with given retry policy
        /// </summary>
        public static Task ExecuteAsync(
            this IRetryPolicy retryPolicy,
            Func<Task> func)
        {
            return ExecuteAsync(
                retryPolicy,
                async () =>
                {
                    await func();
                    return Task.FromResult(0);
                });
        }
    }


}
