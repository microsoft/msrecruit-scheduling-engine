using System;

namespace Microsoft.CommonDataService.Common.Internal
{
    public class TryGetResult<T>
    {
        public TryGetResult(T result, bool succeeded) { }
        public TryGetResult(T result, bool succeeded, Exception e) { }

        public bool Succeeded { get; }
        public Exception Exception { get; }
        public bool HasException { get; }
        public T Result { get; }

        public static implicit operator bool(TryGetResult<T> result)
        {
            // TODO validate
            return true;
        }
    }

    public static class TryGetResult
    {
        // TODO revisit
        public static TryGetResult<T> Create<T>(T result) where T : class {
            return default(T);
        }
        public static TryGetResult<T> CreateFromStruct<T>(T result) where T : struct;
        public static TryGetResult<T?> CreateNullable<T>(T? result) where T : struct;
        public static TryGetResult<T> Failed<T>() where T : class;
        public static TryGetResult<T> Failed<T>(Exception e) where T : class;
        public static TryGetResult<T?> FailedNullable<T>() where T : struct;
        public static TryGetResult<T?> FailedNullable<T>(Exception e) where T : struct;
        public static TryGetResult<T> FailedStruct<T>() where T : struct;
        public static TryGetResult<T> FailedStruct<T>(Exception e) where T : struct;
    }
}
