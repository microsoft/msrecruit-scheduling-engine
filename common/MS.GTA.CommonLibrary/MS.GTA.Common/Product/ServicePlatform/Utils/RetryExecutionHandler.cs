//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// The retry execution handler that executes the function using the specified delay function.
    /// </summary>
    public class RetryExecutionHandler
    {
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
        /// <returns>Returns async Task with TResult.</returns>
        public async static Task<TResult> ExecuteAsync<TResult, TRetryableException>(
            ILogger logger,
            string activityName,
            Func<Task<TResult>> function,
            Func<TResult, bool> canRetryFunction,
            Func<TRetryableException, bool> canRetryOnExceptionFunction,
            DelayFunction delayFunction,
            int maxRetryCount = ContextConstants.Activity.ExponentialBackOffForApiMaxRetryCount) where TRetryableException : Exception
        {
            Contract.CheckValue(function, nameof(function), "Function cannot be null");
            
            var response = default(TResult);
            TRetryableException exceptionToThrowIfRetryFails = null;

            for (int retryAttempt = 0; retryAttempt < maxRetryCount; retryAttempt++)
            {
                exceptionToThrowIfRetryFails = null;

                if (retryAttempt > 0)
                {
                    var timeSpan = delayFunction.GetDelay(retryAttempt);

                    logger.LogInformation($"Retrying the operation: {activityName} Attempt: {retryAttempt} Delay: {timeSpan}.");
                    await Task.Delay(timeSpan);
                }

                try
                {
                    response = await function();

                    if (canRetryFunction == null || !canRetryFunction(response))
                    {
                        return response;
                    }
                }
                catch (TRetryableException retryableException)
                {
                    logger.LogError($"The activity {activityName} threw retryable exception, details: {retryableException}");

                    exceptionToThrowIfRetryFails = retryableException;
                    if (retryAttempt >= maxRetryCount - 1
                        || canRetryOnExceptionFunction == null
                        || !canRetryOnExceptionFunction(retryableException))
                    {
                        throw;
                    }
                }
            }

            if (exceptionToThrowIfRetryFails != null)
            {
                throw exceptionToThrowIfRetryFails;
            }

            return response;
        }
    }
}