//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Diagnostics;

namespace TA.CommonLibrary.CommonDataService.Common.Internal
{
    /// <summary>
    /// Utility methods for object equality comparison.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class EqualityUtil
    {
        /// <summary>
        /// Returns <c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same reference.
        /// Returns <c>false</c> if <paramref name="x"/> or <paramref name="y"/> refer to null, but the other does not.
        /// Returns <c>null</c> otherwise.
        /// </summary>
        [DebuggerStepThrough]
        public static bool? AreEqual<T>(T x, T y) where T : class
        {
            Contract.AssertValueOrNull(x, "x");
            Contract.AssertValueOrNull(y, "y");

            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return null;
        }
    }
}
