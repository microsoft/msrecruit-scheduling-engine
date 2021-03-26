//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Threading;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Utils;
using Microsoft.ServiceFabric.Services.Client;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// Resolution options for Fabric service routers.
    /// </summary>
    public sealed class FabricServiceRouterOptions
    {
        private TimeSpan resolveTimeoutPerTry = TimeSpan.FromSeconds(5.0);  //ServicePartitionResolver.DefaultResolveTimeout;
        private TimeSpan maxResolveRetryBackoffInterval = TimeSpan.FromSeconds(2.0);/// ServicePartitionResolver.DefaultMaxRetryBackoffInterval;

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouterOptions"/> with default values.
        /// </summary>
        public FabricServiceRouterOptions()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouterOptions"/> with values copied from <paramref name="original"/>.
        /// </summary>
        public FabricServiceRouterOptions(FabricServiceRouterOptions original)
        {
            Contract.CheckValue(original, nameof(original));

            ResolveTimeoutPerTry = original.ResolveTimeoutPerTry;
            MaxResolveRetryBackoffInterval = original.MaxResolveRetryBackoffInterval;
            RetryOnNotFound = original.RetryOnNotFound;
        }

        /// <summary>
        /// The timeout passed to FabricClient when resolving service partitions.
        /// 
        /// The default value is <see cref="ServicePartitionResolver.DefaultResolveTimeout"/>.
        /// </summary>
        public TimeSpan ResolveTimeoutPerTry
        {
            get => resolveTimeoutPerTry;
            set
            {
                CheckTimeout(value, nameof(ResolveTimeoutPerTry));
                resolveTimeoutPerTry = value;
            }
        }

        /// <summary>
        /// The max retry back off interval when resolving partitions. The effective delay will be determined at random
        /// by <see cref="IServicePartitionResolver"/> in this range.
        /// 
        /// The default value is <see cref="ServicePartitionResolver.DefaultMaxRetryBackoffInterval"/>.
        /// </summary>
        public TimeSpan MaxResolveRetryBackoffInterval
        {
            get => maxResolveRetryBackoffInterval;
            set
            {
                CheckDelay(value, nameof(MaxResolveRetryBackoffInterval));
                maxResolveRetryBackoffInterval = value;
            }
        }

        /// <summary>
        /// Determines whether to retry on 404 Not Found HTTP responses.
        /// 
        /// The default value is true.
        /// </summary>
        /// <remarks>
        /// Note that the retry only happens if the response does not contain an "X-ServiceFabric" header with "ResourceNotFound" value. In that case
        /// the router will never retry regardless of the <see cref="RetryOnNotFound"/> setting.
        /// </remarks>
        public bool RetryOnNotFound { get; set; } = true;

        /// <summary>
        /// Validates that the provided <paramref name="timeout"/>.
        /// </summary>
        private static void CheckTimeout(TimeSpan timeout, string paramName)
        {
            Contract.CheckRange(
                (timeout == Timeout.InfiniteTimeSpan) ||
                (timeout >= TimeSpan.Zero && timeout <= TimeUtil.MaxCancellationTokenTimeSpan),
                paramName + " must be infinite or between 0 and " + nameof(TimeUtil) + "." + nameof(TimeUtil.MaxCancellationTokenTimeSpan));
        }

        /// <summary>
        /// Validates that the provided <paramref name="delay"/>.
        /// </summary>
        private static void CheckDelay(TimeSpan delay, string paramName)
        {
            Contract.CheckRange(
                delay >= TimeSpan.Zero && delay <= TimeUtil.MaxCancellationTokenTimeSpan,
                paramName + " must be infinite or between 0 and " + nameof(TimeUtil) + "." + nameof(TimeUtil.MaxCancellationTokenTimeSpan));
        }
    }
}
