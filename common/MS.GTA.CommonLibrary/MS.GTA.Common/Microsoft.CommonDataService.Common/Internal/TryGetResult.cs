//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// Factory class for constructing instances of TryGetResult{T}.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class TryGetResult
    {
        public static TryGetResult<T> CreateFromStruct<T>(T result)
            where T : struct
        {
            return new TryGetResult<T>(result, true);
        }

        public static TryGetResult<T?> CreateNullable<T>(T? result)
            where T : struct
        {
            return new TryGetResult<T?>(result, result != null);
        }

        public static TryGetResult<T> Create<T>(T result)
            where T : class
        {
            return new TryGetResult<T>(result, result != null);
        }

        public static TryGetResult<T> FailedStruct<T>()
            where T : struct
        {
            return new TryGetResult<T>(default(T), false);
        }

        public static TryGetResult<T?> FailedNullable<T>()
            where T : struct
        {
            return new TryGetResult<T?>(null, false);
        }

        public static TryGetResult<T> Failed<T>()
            where T : class
        {
            return new TryGetResult<T>(null, false);
        }

        public static TryGetResult<T> FailedStruct<T>(Exception e)
            where T : struct
        {
            return new TryGetResult<T>(default(T), false, e);
        }

        public static TryGetResult<T?> FailedNullable<T>(Exception e)
            where T : struct
        {
            return new TryGetResult<T?>(null, false, e);
        }

        public static TryGetResult<T> Failed<T>(Exception e)
            where T : class
        {
            return new TryGetResult<T>(null, false, e);
        }
    }

    /// <summary>
    /// A utility data contract for passing a boolean and an out parameter (or) an exception from a Try* method.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class TryGetResult<T>
    {
        private readonly T m_result;
        private readonly Exception m_exception;

        public TryGetResult(T result, bool succeeded)
        {
            Succeeded = succeeded;
            m_result = result;
        }

        public TryGetResult(T result, bool succeeded, Exception e)
        {
            Succeeded = succeeded;
            m_result = result;
            m_exception = e;
        }

        public bool Succeeded { get; private set; }

        public Exception Exception
        {
            get { return m_exception; }
        }

        public bool HasException
        {
            get { return m_exception != null; }
        }

        public T Result
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
