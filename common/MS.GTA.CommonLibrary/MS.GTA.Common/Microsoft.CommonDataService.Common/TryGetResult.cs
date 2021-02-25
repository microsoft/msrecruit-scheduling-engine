//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Factory class for constructing instances of TryGetResult{T}.
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    internal static class TryGetResult
    {
        internal static TryGetResult<T> CreateFromStruct<T>(T result)
            where T : struct
        {
            return new TryGetResult<T>(result, true);
        }

        internal static TryGetResult<T?> CreateNullable<T>(T? result)
            where T : struct
        {
            return new TryGetResult<T?>(result, result != null);
        }

        internal static TryGetResult<T> Create<T>(T result)
            where T : class
        {
            return new TryGetResult<T>(result, result != null);
        }

        internal static TryGetResult<T> FailedStruct<T>()
            where T : struct
        {
            return new TryGetResult<T>(default(T), false);
        }

        internal static TryGetResult<T?> FailedNullable<T>()
            where T : struct
        {
            return new TryGetResult<T?>(null, false);
        }

        internal static TryGetResult<T> Failed<T>()
            where T : class
        {
            return new TryGetResult<T>(null, false);
        }

        internal static TryGetResult<T> FailedStruct<T>(Exception e)
            where T : struct
        {
            return new TryGetResult<T>(default(T), false, e);
        }

        internal static TryGetResult<T?> FailedNullable<T>(Exception e)
            where T : struct
        {
            return new TryGetResult<T?>(null, false, e);
        }

        internal static TryGetResult<T> Failed<T>(Exception e)
            where T : class
        {
            return new TryGetResult<T>(null, false, e);
        }
    }

    /// <summary>
    /// A utility data contract for passing a boolean and an out parameter (or) an exception from a Try* method.
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal class TryGetResult<T>
    {
        private readonly T m_result;
        private readonly Exception m_exception;

        internal TryGetResult(T result, bool succeeded)
        {
            Succeeded = succeeded;
            m_result = result;
        }

        internal TryGetResult(T result, bool succeeded, Exception e)
        {
            Succeeded = succeeded;
            m_result = result;
            m_exception = e;
        }

        internal bool Succeeded { get; private set; }

        internal Exception Exception
        {
            get { return m_exception; }
        }

        internal bool HasException
        {
            get { return m_exception != null; }
        }

        internal T Result
        {
            get
            {
                Contract.Check(Succeeded, nameof(Succeeded));
                return m_result;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Won't fix")]
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfinternalMethods", Justification = "Already validated")]
        public static implicit operator bool(TryGetResult<T> result)
        {
            Contract.CheckValue(result, nameof(result));
            return result.Succeeded;
        }
    }
}
