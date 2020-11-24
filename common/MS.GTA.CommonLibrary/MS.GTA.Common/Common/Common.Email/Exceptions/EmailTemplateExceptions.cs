//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email.Exceptions
{
    using System;
    using System.Net;

    using MS.GTA.ServicePlatform.Exceptions;
    
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "Microsoft.D365.HCM.Common.Email.Exceptions", "EmailTemplates_InvalidOperation", MonitoredExceptionKind.Service)]
    [Serializable]
    public sealed class EmailTemplateInvalidOperationException : MonitoredException
    {
        public EmailTemplateInvalidOperationException(string message)
            : base(message)
        {
        }

        public EmailTemplateInvalidOperationException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "Microsoft.D365.HCM.Common.Email.Exceptions", "EmailTemplates_NotFound", MonitoredExceptionKind.Service)]
    [Serializable]
    public sealed class EmailTemplateNotFoundException : MonitoredException
    {
        public EmailTemplateNotFoundException(string emailTemplateId)
            : base($"Email Template: {emailTemplateId} does not exist")
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Microsoft.D365.HCM.Common.Email.Exceptions", "EmailTemplates_NotFound", MonitoredExceptionKind.Service)]
    [Serializable]
    public sealed class NullDocumentClientException : MonitoredException
    {
        public NullDocumentClientException()
            : base($"Document client is null")
        {
        }
    }
}
