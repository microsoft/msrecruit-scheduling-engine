//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Hosting
{
    // These exceptions are exposed on internal APIs only and it is therefore safe to communicate the nature of the error
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, "InvalidExecutionContext", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class InvalidExecutionContextException : MonitoredException
    {
        internal InvalidExecutionContextException(Exception innerException)
            : base("The request execution context is invalid")
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, "MissingExecutionContext", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class MissingExecutionContextException : MonitoredException
    {
        internal MissingExecutionContextException()
            : base("The request execution context is missing")
        {
        }
    }
}
