//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ServicePlatform.Utils
{
    internal static class DefaultMetricConstants
    {
        // Service Platform Metric Namespace
        internal static readonly string MetricNamespace = "ServicePlatform";

        // Custom Dimensions
        internal static readonly string HttpStatusCodeDimension = "Status";

        // Service Platform Default Metric Names
        internal static readonly string IncomingHttpOperationDurationMetricName = "ServicePlatform/IncomingHttpOperationDuration";
        internal static readonly string OutgoingHttpOperationDurationMetricName = "ServicePlatform/OutgoingHttpOperationDuration";
    }
}
