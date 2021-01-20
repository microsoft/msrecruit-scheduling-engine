//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Utils
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
