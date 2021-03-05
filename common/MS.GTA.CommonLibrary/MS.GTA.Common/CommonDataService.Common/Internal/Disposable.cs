//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace CommonDataService.Common.Internal
{
    /// <summary>
    /// An empty <see cref="IDisposable"/> implementation.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class VoidDisposable : IDisposable
    {
        private static VoidDisposable instance = new VoidDisposable();

        private VoidDisposable()
        {
        }

        public static VoidDisposable Instance
        {
            get { return instance; }
        }

        public void Dispose()
        {
        }
    }

    /// <summary>
    /// An action-based <see cref="IDisposable"/> implementation.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class ActionDisposable : IDisposable
    {
        private readonly Action action;
        private int disposed;

        public ActionDisposable(Action action)
        {
            Contract.CheckValue(action, nameof(action));

            this.action = action;
        }

        public void Dispose()
        {
            if (Interlocked.Increment(ref disposed) == 1)
            {
                action();
            }
        }
    }
}
