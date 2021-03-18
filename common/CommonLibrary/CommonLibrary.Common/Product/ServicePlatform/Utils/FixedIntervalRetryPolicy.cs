//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using CommonLibrary.CommonDataService.Common.Internal;

namespace CommonLibrary.ServicePlatform.Utils
{
    /// <summary>
    /// Retry policy that enforces fixed delay between each attempt.  Can limit attempts by maximum number of attempt, maximum duration or both.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class FixedIntervalRetryPolicyBase : IRetryPolicy
    {
        private readonly TimeSpan maxDuration;
        private readonly int maxAttempts;
        private readonly TimeSpan delay;

        public FixedIntervalRetryPolicyBase(TimeSpan delay, int maxAttempts, TimeSpan maxDuration)
        {
            Contract.CheckRange(TimeSpan.Zero <= delay, nameof(delay));
            Contract.CheckRange(maxAttempts >= 0, nameof(maxAttempts));
            Contract.CheckRange(TimeSpan.Zero <= maxDuration, nameof(maxDuration));

            this.delay = delay;
            this.maxAttempts = maxAttempts;
            this.maxDuration = maxDuration;
        }

        public bool ShouldRetry(Exception lastException, int attemptsExecuted, TimeSpan lastAttemptDuration, TimeSpan totalDuration, out TimeSpan nextRetryDelay)
        {
            nextRetryDelay = delay;
            return IsTransient(lastException) && IsWithinPolicyLimits(attemptsExecuted, lastAttemptDuration, totalDuration);
        }

        protected abstract bool IsTransient(Exception lastException);

        private bool IsWithinPolicyLimits(int attemptsExecuted, TimeSpan lastAttemptDuration, TimeSpan totalDuration)
        {
            return attemptsExecuted < maxAttempts && !(totalDuration > maxDuration);
        }
    }

    /// <summary>
    /// Specialized fixed interval policy which only retries if exception is of Type provided.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class FixedIntervalRetryPolicy<TException> : FixedIntervalRetryPolicyBase where TException : Exception
    {
        public FixedIntervalRetryPolicy(TimeSpan delay, int maxAttempts)
            : base(delay, maxAttempts, TimeSpan.MaxValue)
        {
        }

        public FixedIntervalRetryPolicy(TimeSpan delay, TimeSpan maxDuration)
            : base(delay, int.MaxValue, maxDuration)
        {
        }

        public FixedIntervalRetryPolicy(TimeSpan delay, int maxAttempts, TimeSpan maxDuration)
            : base(delay, maxAttempts, maxDuration)
        {
        }

        protected override bool IsTransient(Exception lastException)
        {
            return lastException is TException;
        }
    }
}
