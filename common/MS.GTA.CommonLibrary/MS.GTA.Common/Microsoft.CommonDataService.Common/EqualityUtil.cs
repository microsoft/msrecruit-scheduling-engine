//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------
// NOTE: This file was forked from PowerBI (BI_Azure_Dev: Sql\InfoNav\src\Common\EqualityUtil.cs)
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Utility methods for object equality comparison.
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    internal static class EqualityUtil
    {
        /// <summary>
        /// Returns <c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same reference.
        /// Returns <c>false</c> if <paramref name="x"/> or <paramref name="y"/> refer to null, but the other does not.
        /// Returns <c>null</c> otherwise.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool? AreEqual<T>(T x, T y) where T : class
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
