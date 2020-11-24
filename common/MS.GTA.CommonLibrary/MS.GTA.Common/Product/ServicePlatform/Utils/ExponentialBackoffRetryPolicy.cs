//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// Retry policy that exponentially increases the delay between each retry attempt.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class ExponentialBackoffRetryPolicyBase : IRetryPolicy
    {
        private readonly TimeSpan maxDuration;
        private readonly int maxAttempts;
        private readonly TimeSpan initialDelay;

        public ExponentialBackoffRetryPolicyBase(TimeSpan initialDelay, int maxAttempts, TimeSpan maxDuration)
        {
            Contract.CheckRange(TimeSpan.Zero <= initialDelay, nameof(initialDelay));
            Contract.CheckRange(maxAttempts >= 0, nameof(maxAttempts));
            Contract.CheckRange(TimeSpan.Zero <= maxDuration, nameof(maxDuration));

            this.initialDelay = initialDelay;
            this.maxAttempts = maxAttempts;
            this.maxDuration = maxDuration;
        }

        public bool ShouldRetry(Exception lastException, int attemptsExecuted, TimeSpan lastAttemptDuration, TimeSpan totalDuration, out TimeSpan nextRetryDelay)
        {
            nextRetryDelay = GetNextRetryDelay(attemptsExecuted);
            return IsTransient(lastException) && IsWithinPolicyLimits(attemptsExecuted, lastAttemptDuration, totalDuration);
        }

        protected abstract bool IsTransient(Exception lastException);

        private bool IsWithinPolicyLimits(int attemptsExecuted, TimeSpan lastAttemptDuration, TimeSpan totalDuration)
        {
            return attemptsExecuted < maxAttempts && !(totalDuration > maxDuration);
        }

        private TimeSpan GetNextRetryDelay(int attemptsExecuted)
        {
            return TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * (Math.Pow(2, attemptsExecuted) - 1));
        }
    }

    /// <summary>
    /// Retry policy which only retries if exception is of Type provided.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class ExponentialBackoffRetryPolicy<TException> : ExponentialBackoffRetryPolicyBase where TException : Exception
    {
        public ExponentialBackoffRetryPolicy(TimeSpan initialDelay, int maxAttempts)
            : base(initialDelay, maxAttempts, TimeSpan.MaxValue)
        {
        }

        public ExponentialBackoffRetryPolicy(TimeSpan initialDelay, TimeSpan maxDuration)
            : base(initialDelay, int.MaxValue, maxDuration)
        {
        }

        public ExponentialBackoffRetryPolicy(TimeSpan initialDelay, int maxAttempts, TimeSpan maxDuration)
            : base(initialDelay, maxAttempts, maxDuration)
        {
        }

        protected override bool IsTransient(Exception lastException)
        {
            return lastException is TException;
        }
    }
}
