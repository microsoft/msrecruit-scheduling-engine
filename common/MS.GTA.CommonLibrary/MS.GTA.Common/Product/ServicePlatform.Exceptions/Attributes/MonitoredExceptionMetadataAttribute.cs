//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
////using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Metadata used to define values on a <see cref="MonitoredException"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class MonitoredExceptionMetadataAttribute : Attribute
    {
        [Obsolete("Please use a constructor that accepts error namespace.  This will be removed in packages published after 5/1/2017", false)]
        public MonitoredExceptionMetadataAttribute(HttpStatusCode httpStatusCode, string serviceErrorCode, MonitoredExceptionKind kind)
            : this((int)httpStatusCode, ErrorNamespaces.Default, serviceErrorCode, kind)
        {
        }

        [Obsolete("Please use a constructor that accepts error namespace.  This will be removed in packages published after 5/1/2017", false)]
        public MonitoredExceptionMetadataAttribute(int httpStatusCode, string serviceErrorCode, MonitoredExceptionKind kind)
            : this(httpStatusCode, ErrorNamespaces.Default, serviceErrorCode, kind)
        {
        }

        public MonitoredExceptionMetadataAttribute(HttpStatusCode httpStatusCode, string errorNamespace, string serviceErrorCode, MonitoredExceptionKind kind)
            : this((int)httpStatusCode, errorNamespace, serviceErrorCode, kind)
        {
        }

        public MonitoredExceptionMetadataAttribute(HttpStatusCode httpStatusCode, string errorNamespace, string serviceErrorCode, MonitoredExceptionKind kind, bool propagateError)
            : this((int)httpStatusCode, errorNamespace, serviceErrorCode, kind, propagateError)
        {
        }

        public MonitoredExceptionMetadataAttribute(int httpStatusCode, string errorNamespace, string serviceErrorCode, MonitoredExceptionKind kind)
        {
            // TODO Add null checks
            /*Contract.CheckRange(httpStatusCode >= 100 && httpStatusCode < 600, nameof(httpStatusCode));
            Contract.CheckNonEmpty(serviceErrorCode, nameof(serviceErrorCode));*/

            HttpStatusCode = httpStatusCode;
            ErrorNamespace = errorNamespace;
            ServiceErrorCode = serviceErrorCode;
            Kind = kind;
        }

        public MonitoredExceptionMetadataAttribute(int httpStatusCode, string errorNamespace, string serviceErrorCode, MonitoredExceptionKind kind, bool propagateError)
        {
            // TODO Add null checks
            /*Contract.CheckRange(httpStatusCode >= 100 && httpStatusCode < 600, nameof(httpStatusCode));
            Contract.CheckNonEmpty(serviceErrorCode, nameof(serviceErrorCode));*/

            HttpStatusCode = httpStatusCode;
            ErrorNamespace = errorNamespace;
            ServiceErrorCode = serviceErrorCode;
            Kind = kind;
            PropagateError = propagateError;
        }

        /// <summary>
        /// Gets the HTTP Status Code
        /// </summary>
        public int HttpStatusCode { get; }

        /// <summary>
        /// Gets the Error Namespace
        /// </summary>
        public string ErrorNamespace { get; }

        /// <summary>
        /// Gets the Propagate Error bool which tells services to propagate out the error instead of handling it.
        /// </summary>
        public bool? PropagateError { get; }

        /// <summary>
        /// Gets the Service-Defined Error Identifier
        /// </summary>
        public string ServiceErrorCode { get; }

        /// <summary>
        /// Gets the <see cref="MonitoredExceptionKind"/> of the Exception.
        /// </summary>
        public MonitoredExceptionKind Kind { get; }
    }

    /// <summary>
    /// Value used for unknown exceptions (un-annotated and unmonitored).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class UnknownExceptionMetadataAttribute : MonitoredExceptionMetadataAttribute
    {
        internal UnknownExceptionMetadataAttribute()
            : base(System.Net.HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)
        {
        }
    }
}
