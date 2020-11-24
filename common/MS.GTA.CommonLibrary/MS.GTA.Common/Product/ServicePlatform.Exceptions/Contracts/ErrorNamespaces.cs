//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;

namespace MS.GTA.ServicePlatform.Exceptions
{
    public static class ErrorNamespaces
    {
        /// <summary>
        /// Default namespace to use for monitored exceptions which do not provide a namespace
        /// </summary>
        [Obsolete("Default namespace is only used when the obsolete constructors ServiceError or MonitoredExceptionMetadataAttribute are used.  Use the new constructors which accept error namespace to help prevent naming conflicts.  This will be remove in packages published after 5/1/2017")]
        public const string Default = null;

        /// <summary>
        /// Namespace used for all monitored exceptions defined in Service Platform
        /// </summary>
        public const string ServicePlatform = "ServicePlatform";
    }
}
