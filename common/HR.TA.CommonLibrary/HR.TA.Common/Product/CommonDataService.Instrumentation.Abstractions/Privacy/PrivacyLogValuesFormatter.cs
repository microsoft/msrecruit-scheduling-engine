//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging.Internal;

namespace HR.TA.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Represents a formatter that knows how to mark sensitive information inside of <see cref="FormattedLogValues"/>. 
    /// </summary>
    public class PrivacyLogValuesFormatter
    {
        // Similar caching logic can be found in https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Abstractions/Internal/FormattedLogValues.cs
        internal const int MaxCachedFormatters = 1024;
        private static ConcurrentDictionary<string, LogValuesFormatter> cachedFormatters = new ConcurrentDictionary<string, LogValuesFormatter>();
        private LogValuesFormatter innerFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="PrivacyLogValuesFormatter"/>.
        /// </summary>
        /// <param name="format">Message format.</param>
        public PrivacyLogValuesFormatter(string format)
        {
            if (cachedFormatters.Count >= MaxCachedFormatters)
            {
                if (!cachedFormatters.TryGetValue(format, out innerFormatter))
                {
                    innerFormatter = new LogValuesFormatter(format);
                }
            }
            else
            {
                innerFormatter = cachedFormatters.GetOrAdd(format, f => new LogValuesFormatter(format));
            }
        }

        /// <summary>
        /// Formats the message marking sensitive information.
        /// </summary>
        /// <param name="logValues">Log values to be formatted.</param>
        /// <param name="privacyMarker">Marker for sensitive information.</param>
        /// <returns>Formatted string with sensitive information marked.</returns>
        public string Format(FormattedLogValues logValues, IPrivacyMarker privacyMarker)
        {
            var privacyMarkedValues = new object[logValues.Count];
            for (int i = 0; i < logValues.Count; i++)
            {
                var originalKeyValuePair = logValues[i];
                if (originalKeyValuePair.Value is IPrivateDataContainer privateData)
                {
                    privacyMarkedValues[i] = privacyMarker.ToCompliantValue(privateData);
                }
                else
                {
                    privacyMarkedValues[i] = originalKeyValuePair.Value;
                }
            }

            return this.innerFormatter.Format(privacyMarkedValues);
        }

        /// <summary>
        /// Factory method that extracts original format from <see cref="FormattedLogValues"/>.
        /// </summary>
        /// <param name="logValues">Log values to be formatted.</param>
        /// <param name="privacyMarker">Marker for sensitive information.</param>
        /// <returns>Formatted string with sensitive information marked.</returns>
        public static string CreateAndFormat(FormattedLogValues logValues, IPrivacyMarker privacyMarker)
        {
            // Workaround until https://github.com/aspnet/Logging/issues/295 is fixed.
            const string OriginalFormatPropertyName = "{OriginalFormat}";
            string format = logValues.SingleOrDefault(kvp => kvp.Key.Equals(OriginalFormatPropertyName, StringComparison.OrdinalIgnoreCase)).Value.ToString();

            var formatter = new PrivacyLogValuesFormatter(format);
            return formatter.Format(logValues, privacyMarker);
        }
    }
}
