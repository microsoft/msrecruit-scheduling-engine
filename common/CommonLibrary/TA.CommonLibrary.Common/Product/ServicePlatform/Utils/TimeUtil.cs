//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;

namespace TA.CommonLibrary.ServicePlatform.Utils
{
    /// <summary>
    /// Time and date utilities.
    /// </summary>
    public static class TimeUtil
    {
        /// <summary>
        /// This is the maximum <see cref="TimeSpan"/> that <see cref="CancellationTokenSource.Cancel"/> will accept. Use this
        /// member instead of <see cref="TimeSpan.MaxValue"/> for timeout argument validation.
        /// </summary>
        public static readonly TimeSpan MaxCancellationTokenTimeSpan = TimeSpan.FromMilliseconds(int.MaxValue);
    }
}
