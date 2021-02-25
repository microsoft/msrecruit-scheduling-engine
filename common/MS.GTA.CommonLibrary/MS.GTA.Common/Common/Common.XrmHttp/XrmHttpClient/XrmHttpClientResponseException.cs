//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Privacy;
    using Newtonsoft.Json;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", nameof(XrmHttpClientResponseException), MonitoredExceptionKind.Remote)]
    public class XrmHttpClientResponseException : MonitoredException
    {
        // Error codes
        // https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/org-service/web-service-error-codes
        public const string ZeroErrorCode = "0x0";
        public const string GenericUnexpectedErrorCode = "0x80040216";
        public const string CrmHttpErrorCode = "0x8006088a";
        public const string ValidationFailedErrorCode = "0x80044331";
        public const string PluginUnexpectedErrorCode = "0x80040224";
        public const string PluginAbortedErrorCode = "0x80040265";
        public const string DuplicateKeyValuesErrorCode = "0x80040237";
        public const string DuplicateAlternateKeyValuesErrorCode = "0x80060892";
        public const string NoUserRolesAssignedErrorCode = "0x80042f09";
        public const string UserDisabledErrorCode = "0x80041d1f";
        public const string InvalidCharactersInFieldErrorCode = "0x80040278";
        public const string AggregateQueryRecordLimitExceededErrorCode = "0x8004e023";

        public const HttpStatusCode TooManyRequests = (HttpStatusCode)429;

        // Stack trace method names
        public const string AKResolveStackTrace = "AlternateKeysODataUriResolver";
        public const string SandboxExecuteStackTrace = "Microsoft.Crm.Sandbox.SandboxWorker.Execute";

        // Exception names
        public const string ODataUnrecognizedPathExceptionName = "ODataUnrecognizedPathException";

        // Error message substrings
        public const string CouldNotFindPropertyNamedMessage = "Could not find a property named ";
        public const string UntypedValueInNonOpenTypeMessage = "Does not support untyped value in non-open type";
        public const string RelevanceSearchApiDisabledMessage = "The Relevance Search API is currently disabled";
        public const string RequestHeadersTooLongMessage = "The size of the request headers is too long";

        // Assembly names
        public const string TalentXrmDllAssemblyPrefix = "Microsoft.Dynamics.HCM.XrmSolutions.Talent";

        public XrmHttpClientResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null)
            : base($"Request (REQ_ID={reqId}{(string.IsNullOrEmpty(authTicketId) ? string.Empty : $", AuthActivityId={authTicketId}")}) failed with error {statusCode} ({reasonPhrase}): {contentString}")
        {
            this.StatusCode = statusCode;
            this.ReasonPhrase = reasonPhrase;
            this.RequestId = reqId;
            this.AuthenticationTicketId = authTicketId;

            try
            {
                var responseBody = JsonConvert.DeserializeObject<XrmErrorResponseBody>(contentString);
                this.RemoteCode = responseBody.Error?.Code;
                this.RemoteMessage = responseBody.Error?.Message ?? responseBody.Message;
                this.RemoteType = responseBody.Error?.InnerError?.Type;
                this.RemoteStackTrace = responseBody.Error?.InnerError?.StackTrace;
            }
            catch
            {
                // Do nothing.
            }
        }

        public static XrmHttpClientResponseException FromResponse(HttpResponseMessage response, string contentString)
        {
            var statusCode = response.StatusCode;
            var reasonPhrase = response.ReasonPhrase;
            response.Headers.TryGetValues("REQ_ID", out var reqIdHeaders);
            response.Headers.TryGetValues("x-authentication-ticketid", out var authTicketIdHeaders);
            response.Headers.TryGetValues("AuthActivityId", out var authActivityIdHeaders);
            response.Dispose();

            var reqId = reqIdHeaders?.FirstOrDefault();
            var authTicketId = authTicketIdHeaders?.FirstOrDefault() ?? authActivityIdHeaders?.FirstOrDefault();

            XrmErrorResponseBody responseBody = null;
            if (!string.IsNullOrEmpty(contentString))
            {
                try
                {
                    responseBody = JsonConvert.DeserializeObject<XrmErrorResponseBody>(contentString);
                }
                catch (Exception)
                {
                    // Do nothing.
                }
            }

            if (responseBody != null)
            {
                if (XrmHttpClientMissingAKException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientMissingAKException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientUnrecognizedPathException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientUnrecognizedPathException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientUnknownPropertyException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientUnknownPropertyException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientValidationFailedException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientValidationFailedException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientDuplicateKeyValuesException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientDuplicateKeyValuesException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientDuplicateAlternateKeyValuesException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientDuplicateAlternateKeyValuesException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientNoUserRolesAssignedException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientNoUserRolesAssignedException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientUserDisabledException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientUserDisabledException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientRelevanceSearchNotEnabledException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientRelevanceSearchNotEnabledException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientInvalidCharactersInFieldException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientInvalidCharactersInFieldException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientCustomerPluginException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientCustomerPluginException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }

                if (XrmHttpClientPluginExecutionFailedException.IsError(statusCode, reasonPhrase, responseBody))
                {
                    return new XrmHttpClientPluginExecutionFailedException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                }
            }

            if (XrmHttpClientRequestHeadersTooLongException.IsError(statusCode, reasonPhrase, contentString))
            {
                return new XrmHttpClientRequestHeadersTooLongException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
            }

            switch (statusCode)
            {
                case HttpStatusCode.BadGateway:
                    return new XrmHttpClientBadGatewayResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.BadRequest:
                    return new XrmHttpClientBadRequestResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.Conflict:
                    return new XrmHttpClientConflictResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.Forbidden:
                    return new XrmHttpClientForbiddenResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.InternalServerError:
                    return new XrmHttpClientInternalServerErrorResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.NotFound:
                    return new XrmHttpClientNotFoundResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.PreconditionFailed:
                    return new XrmHttpClientPreconditionFailedResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case HttpStatusCode.Unauthorized:
                    return new XrmHttpClientUnauthorizedResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
                case TooManyRequests:
                    return new XrmHttpClientTooManyRequestsResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
            }

            if (responseBody != null && XrmHttpClientOrganizationServiceFaultException.IsOrganizationServiceFaultError(statusCode, reasonPhrase, responseBody))
            {
                return new XrmHttpClientOrganizationServiceFaultException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
            }

            return new XrmHttpClientResponseException(statusCode, reasonPhrase, contentString, reqId, authTicketId);
        }

        [ExceptionCustomData(Serialize = true)]
        public HttpStatusCode StatusCode { get; }

        [ExceptionCustomData(Serialize = true)]
        public string ReasonPhrase { get; }

        [ExceptionCustomData(Serialize = true)]
        public string RequestId { get; }

        [ExceptionCustomData(Serialize = true)]
        public string AuthenticationTicketId { get; private set; }

        [ExceptionCustomData(Serialize = true)]
        public string RemoteCode { get; }

        [ExceptionCustomData(Serialize = true)]
        public string RemoteMessage { get; }

        [ExceptionCustomData(Serialize = true)]
        public string RemoteType { get; }

        public string RemoteStackTrace { get; }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMBadRequest", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientBadRequestResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientBadRequestResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.GTA.Common.XrmHttp", "XRMNotFound", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientNotFoundResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientNotFoundResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.Unauthorized, "MS.GTA.Common.XrmHttp", "XRMUnauthorized", MonitoredExceptionKind.Benign)]
    public class XrmHttpClientUnauthorizedResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientUnauthorizedResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = MS.GTA.Common.Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.GTA.Common.XrmHttp", "XRMForbidden", MonitoredExceptionKind.Benign)]
    public class XrmHttpClientForbiddenResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientForbiddenResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = MS.GTA.Common.Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    [MonitoredExceptionMetadata(HttpStatusCode.BadGateway, "MS.GTA.Common.XrmHttp", "XRMBadGateway", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientBadGatewayResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientBadGatewayResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMPreconditionFailed", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientPreconditionFailedResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientPreconditionFailedResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMTooManyRequests", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientTooManyRequestsResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientTooManyRequestsResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMConflict", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientConflictResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientConflictResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMInternalServerError", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientInternalServerErrorResponseException : XrmHttpClientResponseException
    {
        public XrmHttpClientInternalServerErrorResponseException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", nameof(XrmHttpClientCustomerPluginException), MonitoredExceptionKind.Benign)]
    public class XrmHttpClientCustomerPluginException : XrmHttpClientResponseException
    {
        public XrmHttpClientCustomerPluginException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            var isErrorFromUnexpectedPluginTermination = responseBody?.Error?.Code?.Equals(PluginUnexpectedErrorCode, StringComparison.InvariantCultureIgnoreCase) == true
                && responseBody?.Error?.Message?.Contains(TalentXrmDllAssemblyPrefix) != true;
            var isErrorFromPluginAbortingExecution = responseBody?.Error?.Code?.Equals(PluginAbortedErrorCode, StringComparison.InvariantCultureIgnoreCase) == true
                && responseBody?.Error?.Message?.Contains(TalentXrmDllAssemblyPrefix) != true;
            return isErrorFromUnexpectedPluginTermination || isErrorFromPluginAbortingExecution;
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = MS.GTA.Common.Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMPluginExecutionFailedException", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientPluginExecutionFailedException : XrmHttpClientInternalServerErrorResponseException
    {
        public XrmHttpClientPluginExecutionFailedException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.InternalServerError
                && (responseBody?.Error?.InnerError?.StackTrace?.Contains(SandboxExecuteStackTrace) == true
                 || responseBody?.Error?.InnerError?.Message?.Contains(SandboxExecuteStackTrace) == true);
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XrmMissingAK", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientMissingAKException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientMissingAKException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && (responseBody?.Error?.Code?.Equals(CrmHttpErrorCode, StringComparison.InvariantCultureIgnoreCase) == true
                 || responseBody?.Error?.Code?.Equals(ZeroErrorCode, StringComparison.InvariantCultureIgnoreCase) == true)
                && responseBody?.Error?.InnerError?.StackTrace?.Contains(AKResolveStackTrace) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XrmRequestHeadersTooLong", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientRequestHeadersTooLongException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientRequestHeadersTooLongException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, string responseString)
        {
            return statusCode == HttpStatusCode.BadRequest
                && responseString?.Contains(RequestHeadersTooLongMessage) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMUnrecognizedPath", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientUnrecognizedPathException : XrmHttpClientNotFoundResponseException
    {
        public XrmHttpClientUnrecognizedPathException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.NotFound
                && responseBody?.Error?.Code?.Equals(CrmHttpErrorCode, StringComparison.InvariantCultureIgnoreCase) == true
                && responseBody?.Error?.InnerError?.Type?.Contains(ODataUnrecognizedPathExceptionName) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMUnknownProperty", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientUnknownPropertyException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientUnknownPropertyException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && (responseBody?.Error?.Message?.StartsWith(CouldNotFindPropertyNamedMessage) == true
                 || responseBody?.Error?.Message?.Contains(UntypedValueInNonOpenTypeMessage) == true);
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.Common.XrmHttp", "XRMValidationFailed", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientValidationFailedException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientValidationFailedException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && responseBody?.Error?.Code?.Equals(ValidationFailedErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.Common.XrmHttp", "XRMInvalidCharactersInField", MonitoredExceptionKind.Benign)]
    public class XrmHttpClientInvalidCharactersInFieldException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientInvalidCharactersInFieldException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && responseBody?.Error?.Code?.Equals(InvalidCharactersInFieldErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.GTA.Common.XrmHttp", "XRMUserDisabled", MonitoredExceptionKind.Benign)]
    public class XrmHttpClientUserDisabledException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientUserDisabledException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && responseBody?.Error?.Code?.Equals(UserDisabledErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMDuplicateKeyValues", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientDuplicateKeyValuesException : XrmHttpClientPreconditionFailedResponseException
    {
        public XrmHttpClientDuplicateKeyValuesException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.PreconditionFailed
                && responseBody?.Error?.Code?.Equals(DuplicateKeyValuesErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMDuplicateAlternateKeyValues", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientDuplicateAlternateKeyValuesException : XrmHttpClientPreconditionFailedResponseException
    {
        public XrmHttpClientDuplicateAlternateKeyValuesException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.PreconditionFailed
                && responseBody?.Error?.Code?.Equals(DuplicateAlternateKeyValuesErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.GTA.Common.XrmHttp", "XRMNoUserRolesAssigned", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientNoUserRolesAssignedException : XrmHttpClientForbiddenResponseException
    {
        public XrmHttpClientNoUserRolesAssignedException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.Forbidden
                && responseBody?.Error?.Code?.Equals(NoUserRolesAssignedErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XRMOrganizationServiceFault", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientOrganizationServiceFaultException : XrmHttpClientResponseException
    {
        public XrmHttpClientOrganizationServiceFaultException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsOrganizationServiceFaultError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return responseBody?.Error?.InnerError?.Type?.Contains("OrganizationServiceFault") == true;
        }
    }

    [MonitoredExceptionMetadata(HttpStatusCode.NotImplemented, "MS.GTA.Common.XrmHttp", "XRMRelevanceSearchNotAllowed", MonitoredExceptionKind.Benign)]
    public class XrmHttpClientRelevanceSearchNotEnabledException : XrmHttpClientResponseException
    {
        public XrmHttpClientRelevanceSearchNotEnabledException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.NotImplemented
                && responseBody?.Message.Contains(RelevanceSearchApiDisabledMessage) == true;
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = MS.GTA.Common.Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    [MonitoredExceptionMetadata(HttpStatusCode.NotImplemented, "MS.GTA.Common.XrmHttp", "XRMFetchXMLAggregateQueryRecordLimitExceeded", MonitoredExceptionKind.Remote)]
    public class XrmHttpClientFetchXmlAggregateQueryRecordLimitExceededException : XrmHttpClientBadRequestResponseException
    {
        public XrmHttpClientFetchXmlAggregateQueryRecordLimitExceededException(HttpStatusCode statusCode, string reasonPhrase, string contentString, string reqId = null, string authTicketId = null) : base(statusCode, reasonPhrase, contentString, reqId, authTicketId)
        {
        }

        public static bool IsError(HttpStatusCode statusCode, string reasonPhrase, XrmErrorResponseBody responseBody)
        {
            return statusCode == HttpStatusCode.BadRequest
                && responseBody?.Error?.Code?.Equals(AggregateQueryRecordLimitExceededErrorCode, StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }
}
