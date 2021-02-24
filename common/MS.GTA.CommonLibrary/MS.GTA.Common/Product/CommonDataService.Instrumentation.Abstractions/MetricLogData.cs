//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.CommonDataService.Instrumentation
{
    /// <summary>
    /// Container for metric data. This allows implementation of ILogger to detect input type and perform corresponding operation.
    /// </summary>
    public sealed class MetricLogData
    { 
        public string MetricName { get; }

        internal string MetricNamespace { get; }

        public IDictionary<string, string> MetricDimensions { get; }

        public long Value { get; }

        public MetricLogData(string metricName, long value)
            : this(metricName, (IDictionary<string, string>)null, value)
        {
        }

        internal MetricLogData(string metricName, string metricNamespace, IDictionary<string, string> metricDimensions, long value)
            : this(metricName, metricDimensions, value)
        {
            Contract.CheckNonEmpty(metricNamespace, nameof(metricNamespace));
            MetricNamespace = metricNamespace;
        }

        public MetricLogData(string metricName, IDictionary<string, string> metricDimensions, long value)
        {
            Contract.CheckNonEmpty(metricName, nameof(metricName));
            Contract.CheckRange(value >= 0, nameof(value));
            
            MetricName = metricName;
            MetricDimensions = metricDimensions;
            Value = value;
        }
    }
}
