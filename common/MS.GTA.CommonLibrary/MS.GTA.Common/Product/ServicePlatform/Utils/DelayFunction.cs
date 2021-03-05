//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using CommonDataService.Common.Internal;

namespace ServicePlatform.Utils
{
    /// <summary>
    /// Base class for delay functions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class DelayFunction
    {
        /// <summary>
        /// A shared instance of zero constant delay function (i.e. no delay)
        /// </summary>
        public static readonly DelayFunction Zero = new ConstantDelay(TimeSpan.Zero);
        private static readonly Random random = new Random();

        /// <summary>
        /// Initializes an instance of <see cref="DelayFunction"/> with no spread.
        /// </summary>
        public DelayFunction()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="DelayFunction"/> with a spread.
        /// </summary>
        /// <param name="spread">
        /// Defines the function spread percentage in the range 0..1, inclusive on both ends. The percentage
        /// defines a random spread that will be applied to the original delay <see cref="TimeStamp"/> on
        /// either side.
        /// 
        /// Example:
        /// 
        /// A delay of 1600 milliseconds with <paramref name="spread"/> of 0.5 will result in a random delay
        /// in the range 800..2400 milliseconds.
        /// </param>
        public DelayFunction(double spread)
        {
            Contract.CheckRange(spread >= 0 && spread <= 1, nameof(spread) + " must be in range 0..1");
            Spread = spread;
        }

        // Internal, for test only
        internal double Spread { get; }

        /// <summary>
        /// Gets the delay for the provided <paramref name="retryAttempt"/>.
        /// </summary>
        /// <param name="retryAttempt">The retry attempt (1, 2, 3, ...). Must be greater or equal to 1.</param>
        public TimeSpan GetDelay(int retryAttempt)
        {
            Contract.CheckRange(retryAttempt >= 1, nameof(retryAttempt));

            var delay = GetSpecificDelay(retryAttempt);
            CheckValidDelay(delay, nameof(delay));

            if (delay != TimeSpan.Zero && Spread > 0)
            {
                var adjustmentModifier = (2 * random.NextDouble()) - 1; // Random number in range <-1, 1>
                delay += TimeSpan.FromMilliseconds(adjustmentModifier * Spread * delay.TotalMilliseconds);
            }

            return delay;
        }

        /// <summary>
        /// Implementing classes provide their specific function.
        /// </summary>
        /// <param name="retryAttempt">The retry attempt (1, 2, 3, ...)</param>
        protected abstract TimeSpan GetSpecificDelay(int retryAttempt);

        protected static void CheckValidDelay(TimeSpan delay, string paramName)
        {
            Contract.CheckRange(delay >= TimeSpan.Zero && delay.TotalMilliseconds <= int.MaxValue, paramName);
        }
    }

    /// <summary>
    /// Represents a constant delay function.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class ConstantDelay : DelayFunction
    {
        private readonly TimeSpan delay;

        /// <summary>
        /// Creates a new instance of <see cref="ConstantDelay"/> function.
        /// </summary>
        /// <param name="delay">The constant delay</param>
        public ConstantDelay(TimeSpan delay)
            : this(delay, spread: 0)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConstantDelay"/> function.
        /// </summary>
        /// <param name="delay">The constant delay</param>
        /// <param name="spread">The <see cref="DelayFunction"/> spread percentage.</param>
        public ConstantDelay(TimeSpan delay, double spread)
            : base(spread)
        {
            CheckValidDelay(delay, nameof(delay));

            this.delay = delay;
        }

        /// <inheritdoc />
        protected override TimeSpan GetSpecificDelay(int retryAttempt)
        {
            return delay;
        }
    }

    /// <summary>
    /// Represents an exponential back off delay function.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class ExponentialBackoffDelay : DelayFunction
    {
        private readonly double initialDelayMilliseconds;
        private readonly double maxDelayMilliseconds;

        /// <summary>
        /// Creates a new instance of <see cref="ExponentialBackoffDelay"/> function.
        /// </summary>
        /// <param name="initialDelay">The delay for the initial retry attempt.</param>
        /// <param name="maxDelay">The max delay for any attempt.</param>
        public ExponentialBackoffDelay(TimeSpan initialDelay, TimeSpan maxDelay)
            : this(initialDelay, maxDelay, spread: 0)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExponentialBackoffDelay"/> function.
        /// </summary>
        /// <param name="initialDelay">The delay for the initial retry attempt.</param>
        /// <param name="maxDelay">The max delay for any attempt.</param>
        /// <param name="spread">The <see cref="DelayFunction"/> spread percentage.</param>
        public ExponentialBackoffDelay(TimeSpan initialDelay, TimeSpan maxDelay, double spread)
            : base(spread)
        {
            Contract.CheckRange(initialDelay > TimeSpan.Zero && initialDelay.TotalMilliseconds <= int.MaxValue, nameof(initialDelay));
            Contract.CheckRange(maxDelay >= initialDelay && maxDelay.TotalMilliseconds <= int.MaxValue, nameof(maxDelay));

            initialDelayMilliseconds = initialDelay.TotalMilliseconds;
            maxDelayMilliseconds = maxDelay.TotalMilliseconds;
        }

        /// <inheritdoc />
        protected override TimeSpan GetSpecificDelay(int retryAttempt)
        {
            var retryMilliseconds = Math.Min(
                maxDelayMilliseconds,
                initialDelayMilliseconds * Math.Pow(2, retryAttempt - 1));

            return TimeSpan.FromMilliseconds(retryMilliseconds);
        }
    }

    /// <summary>
    /// Represents a composite delay of constant times and a function. 
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class CompositeDelay : DelayFunction
    {
        private readonly TimeSpan[] initialDelays;
        private readonly DelayFunction nextDelays;

        /// <summary>
        /// Creates a new instance of <see cref="CompositeDelay"/> function.
        /// </summary>
        /// <param name="initialDelays">The set of initial delays.</param>
        /// <param name="nextDelays">The <see cref="DelayFunction"/> to use on further attempts. Note that the retryAttempt counter
        /// will start at 1 after exhausting the list of <paramref name="initialDelays"/>.</param>
        public CompositeDelay(TimeSpan[] initialDelays, DelayFunction nextDelays)
            : this(initialDelays, nextDelays, spread: 0)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="CompositeDelay"/> function.
        /// </summary>
        /// <param name="initialDelays">The set of initial delays.</param>
        /// <param name="nextDelays">The <see cref="DelayFunction"/> to use on further attempts. Note that the retryAttempt counter
        /// will start at 1 after exhausting the list of <paramref name="initialDelays"/>.</param>
        /// <param name="spread">The <see cref="DelayFunction"/> spread percentage.</param>
        public CompositeDelay(TimeSpan[] initialDelays, DelayFunction nextDelays, double spread)
            : base(spread)
        {
            Contract.CheckValue(initialDelays, nameof(initialDelays));
            Contract.CheckNonEmpty(initialDelays, nameof(initialDelays));
            Contract.CheckValue(nextDelays, nameof(nextDelays));
            for (int i = 0; i < initialDelays.Length; i++)
            {
                CheckValidDelay(initialDelays[i], nameof(initialDelays));
            }

            this.initialDelays = initialDelays;
            this.nextDelays = nextDelays;
        }

        /// <inheritdoc />
        protected override TimeSpan GetSpecificDelay(int retryAttempt)
        {
            int initialDelayIndex = retryAttempt - 1;

            if (initialDelayIndex < initialDelays.Length)
            {
                return initialDelays[initialDelayIndex];
            }

            return nextDelays.GetDelay(retryAttempt - initialDelays.Length);
        }
    }
}
