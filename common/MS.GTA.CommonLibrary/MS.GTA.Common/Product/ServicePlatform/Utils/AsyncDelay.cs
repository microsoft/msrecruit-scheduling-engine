// ----------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// Contract for asynchronous delay implementations.
    /// </summary>
    public interface IAsyncDelay
    {
        /// <summary>
        /// Returns a delay task given the provided <paramref name="delay"/>.
        /// </summary>
        Task Delay(TimeSpan delay, CancellationToken cancellationToken);
    }

    /// <summary>
    /// An implementation of <see cref="IAsyncDelay"/> over <see cref="Task.Delay(TimeSpan, CancellationToken)"/>
    /// </summary>
    public sealed class TaskAsyncDelay : IAsyncDelay
    {
        /// <inheritdoc />
        public Task Delay(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Delay(delay, cancellationToken);
        }
    }
}
