//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Data.Utils
{
    using System;

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get Epoch time in seconds.
        /// </summary>
        /// <param name="dateTime">Date time to calculate epoch for</param>
        /// <returns>Time in seconds.</returns>
        public static long ToEpoch(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1);
            var epochTimeSpan = dateTime - epoch;
            return (long)epochTimeSpan.TotalSeconds;
        }

        /// <summary>
        /// GEt Date time from epoch second
        /// </summary>
        /// <param name="timestamp">Time stamp</param>
        /// <returns>Date time</returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }
    }
}
