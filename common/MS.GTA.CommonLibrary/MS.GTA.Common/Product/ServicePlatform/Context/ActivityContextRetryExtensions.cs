//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace ServicePlatform.Context.Retry
{
    /// <summary>
    /// The Activity context extension class to retry tasks using supported delay function.
    /// </summary>
    public static class ActivityContextRetryExtensions
    {
        /// <summary>
        /// Executes the request in retry mode asynchronously using exponential back off delay.
        /// The method supports retry function for response (of type TResult) and exception (of type TRetryableException).
        /// </summary>
        /// <param name="logger">The extension of ILogger interface.</param>
        /// <param name="activityName">The name of the activity.</param>
        /// <param name="function">The retry function to be executed with retryable exception as input and returns bool.</param>
        /// <param name="canRetryFunction">Function pointer to set boolean to retry.</param>
        /// <param name="canRetryOnException">Function pointer to set boolean to retry.</param>
        /// <param name="initialDelayInMilliSeconds">The initial delay of exponential back off in seconds.</param>
        /// <param name="maxDelayInMilliSeconds">The max delay of exponential back off in seconds {The usual timeout setting for API call is 120s, keeping the max delay to 90s}.</param>
        /// <param name="maxRetryCount">Specify the maximum retry count value, Default: 5 Delay pattern {1, 4, 8, 16, 32} seconds.</param>
        /// <param name="exceptionTypesToIgnore">The list of exceptions to ignore for telemetry.</param>
        /// <returns>Returns async Task with TResult.</returns>
        public async static Task<TResult> ExecuteWithExponentialBackOffRetryOnExceptionAsync<TResult, TRetryableException>(
            this ILogger logger,
            string activityName,
            Func<Task<TResult>> function,
            Func<TResult, bool> canRetryFunction,
            Func<TRetryableException, bool> canRetryOnException,
            int initialDelayInMilliSeconds = ContextConstants.Activity.ExponentialBackOffForApiInitialDelayInMilliSeconds,
            int maxDelayInMilliSeconds = ContextConstants.Activity.ExponentialBackOffForApiMaxDelayInMilliSeconds,
            int maxRetryCount = ContextConstants.Activity.ExponentialBackOffForApiMaxRetryCount,
            Type[] exceptionTypesToIgnore = null) where TRetryableException : Exception
        {
            return await logger.ExecuteWithRetryOnExceptionAsync(activityName,
                function,
                canRetryFunction,
                canRetryOnException,
                new ExponentialBackoffDelay(TimeSpan.FromMilliseconds(initialDelayInMilliSeconds), TimeSpan.FromMilliseconds(maxDelayInMilliSeconds)),
                maxRetryCount,
                exceptionTypesToIgnore);
        }

        /// <summary>
        /// Executes the request in retry mode asynchronously using exponential back off delay retry.
        /// The method supports retry function for response (TResult) only.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="logger">The extension of ILogger interface.</param>
        /// <param name="activityName">The name of the activity.</param>
        /// <param name="function">The retry function to be executed with TResult as input and returns bool.</param>
        /// <param name="canRetryFunction">Function pointer to set boolean to retry.</param>
        /// <param name="initialDelayInMilliSeconds">The initial delay of exponential back off in seconds.</param>
        /// <param name="maxDelayInMilliSeconds">The max delay of exponential back off in seconds {The usual timeout setting for API call is 120s, keeping the max delay to 90s}.</param>
        /// <param name="maxRetryCount">Specify the maximum retry count value, Default: 5 Delay pattern {1, 4, 8, 16, 32} seconds.</param>
        /// <param name="exceptionTypesToIgnore">The list of exceptions to ignore for telemetry.</param>
        /// <returns>Returns async Task with TResult.</returns>
        public async static Task<TResult> ExecuteWithExponentialBackOffRetryAsync<TResult>(
            this ILogger logger,
            string activityName,
            Func<Task<TResult>> function,
            Func<TResult, bool> canRetryFunction,
            int initialDelayInMilliSeconds = ContextConstants.Activity.ExponentialBackOffForApiInitialDelayInMilliSeconds,
            int maxDelayInMilliSeconds = ContextConstants.Activity.ExponentialBackOffForApiMaxDelayInMilliSeconds,
            int maxRetryCount = ContextConstants.Activity.ExponentialBackOffForApiMaxRetryCount,
            Type[] exceptionTypesToIgnore = null)
        {
            return await logger.ExecuteWithRetryAsync<TResult>(activityName,
                    function,
                    canRetryFunction,
                    new ExponentialBackoffDelay(TimeSpan.FromMilliseconds(initialDelayInMilliSeconds), TimeSpan.FromMilliseconds(maxDelayInMilliSeconds), 0.1),
                    maxRetryCount
                );
        }

        /// <summary>
        /// Executes the request in retry mode asynchronously for the specified delay function.
        /// The method supports retry function for response (TResult) only.
        /// </summary>
        /// <param name="logger">The extension of ILogger interface.</param>
        /// <param name="activityName">The name of the activity.</param>
        /// <param name="function">The retry function to be executed with TResult as input and returns bool.</param>
        /// <param name="canRetryFunction">Function pointer to set boolean to retry.</param>
        /// <param name="maxRetryCount">Specify the maximum retry count value.</param>
        /// <param name="delayFunction">The delay function that specifies what type of delay function to use {ConstantDelay, ExponentialBackoffDelay, CompositeDelay}.</param>
        /// <param name="exceptionTypesToIgnore">The list of exceptions to ignore for telemetry.</param>
        /// <returns>Returns async Task with TResult.</returns>
        public async static Task<TResult> ExecuteWithRetryAsync<TResult>(
            this ILogger logger,
            string activityName,
            Func<Task<TResult>> function,
            Func<TResult, bool> canRetryFunction,
            DelayFunction delayFunction,
            int maxRetryCount = ContextConstants.Activity.ExponentialBackOffForApiMaxRetryCount,
            Type[] exceptionTypesToIgnore = null)
        {
            return await logger.ExecuteWithRetryOnExceptionAsync<TResult, Exception>(activityName,
                function,
                canRetryFunction,
                null,   // set exception to null, as there is no exception expected.
                delayFunction,
                maxRetryCount,
                exceptionTypesToIgnore);
        }

        /// <summary>
        /// Executes the request in retry mode asynchronously using specific delay function.
        /// The method supports retry function for response (of type TResult) and exception (of type TRetryableException).
        /// </summary>
        /// <param name="logger">The extension of ILogger interface.</param>
        /// <param name="activityName">The name of the activity.</param>
        /// <param name="function">The retry function to be executed with retryable exception as input and returns bool.</param>
        /// <param name="canRetryFunction">Function pointer to set boolean to retry.</param>
        /// <param name="canRetryOnExceptionFunction">Function pointer to set boolean to retry.</param>
        /// <param name="maxRetryCount">Specify the maximum retry count value.</param>
        /// <param name="delayFunction">The delay function that specifies what type of delay function to use {ConstantDelay, ExponentialBackoffDelay, CompositeDelay}.</param>
        /// <param name="exceptionTypesToIgnore">The list of exceptions to ignore for telemetry.</param>
        /// <returns>Returns async Task with TResult.</returns>
        public async static Task<TResult> ExecuteWithRetryOnExceptionAsync<TResult, TRetryableException>(
            this ILogger logger,
            string activityName,
            Func<Task<TResult>> function,
            Func<TResult, bool> canRetryFunction,
            Func<TRetryableException, bool> canRetryOnExceptionFunction,
            DelayFunction delayFunction,
            int maxRetryCount = ContextConstants.Activity.ExponentialBackOffForApiMaxRetryCount,
            Type[] exceptionTypesToIgnore = null) where TRetryableException : Exception
        {
            return await logger.ExecuteAsync(activityName, 
                async () => 
                {
                    return await RetryExecutionHandler.ExecuteAsync<TResult, TRetryableException>(logger,
                        activityName,
                        function,
                        canRetryFunction,
                        canRetryOnExceptionFunction,
                        delayFunction,
                        maxRetryCount);
                }, 
                exceptionTypesToIgnore);
        }
    }
}
