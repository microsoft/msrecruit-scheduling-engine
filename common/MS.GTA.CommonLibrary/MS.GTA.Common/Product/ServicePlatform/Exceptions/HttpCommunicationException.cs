//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    using System.ComponentModel;
    using Microsoft.Extensions.Logging;
    using Tracing;

    /// <summary>
    /// Monitored wrapper around HTTP communication exceptions.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public class HttpCommunicationException : MonitoredException
    {
        internal HttpCommunicationException()
            : base(message: null, innerException: null)
        {
        }

        internal HttpCommunicationException(string message)
            : base(message, innerException: null)
        {
        }

        internal HttpCommunicationException(Exception innerException)
            : base("An HTTP communication error occurred", innerException)
        {
        }

        internal HttpCommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal HttpCommunicationException(string message, ServiceError serviceError, Exception innerException)
            : base(message, serviceError, innerException)
        {
        }

        /// <summary>
        /// Attempts to translate the provided <paramref name="exception"/> into a more specific <see cref="HttpCommunicationException"/>.
        /// 
        /// Returns true if the conversion is possible or if the provided <see cref="exception"/> is already an instance of 
        /// <see cref="HttpCommunicationException"/>. Otherwise returns false.
        /// </summary>
        public static bool TryCreate(Exception exception, out HttpCommunicationException httpException)
        {
            Contract.CheckValue(exception, nameof(exception));

            if (exception is HttpCommunicationException)
            {
                httpException = (HttpCommunicationException)exception;
                return true;
            }

            if (exception is HttpRequestException)
            {
                httpException = Create((HttpRequestException)exception);
                return true;
            }

            httpException = null;
            return false;
        }

        /// <summary>
        /// Wraps the provided <paramref name="exception"/> in a monitored equivalent derived from
        /// <see cref="HttpCommunicationException"/> based on the following logic:
        /// 
        /// - If the provided <paramref name="exception"/> is a WebException or it contains a WebException anywhere in its stack of inner exceptions then
        ///     - If the WebExceptionStatus is one of the following
        ///         - WebExceptionStatus.ConnectFailure
        ///         - WebExceptionStatus.NameResolutionFailure
        ///         - WebExceptionStatus.ProxyNameResolutionFailure
        ///       then an <see cref="HttpEndpointUnreachableException"/> is returned
        ///     - For all other WebExceptionStatus values, a <see cref="HttpCommunicationWebException"/> is returned
        /// - In all other cases an <see cref="HttpCommunicationException"/> is returned
        /// 
        /// The returned exception always wraps the original input <paramref name="exception"/>.
        /// </summary>
        public static HttpCommunicationException Create(HttpRequestException exception, ILogger logger = null)
        {
            Contract.CheckValue(exception, nameof(exception));
             
            Log($"Handling http communication exception, generic exception is {exception.Message} and of details {exception}", logger);

            var webException = exception.FlattenHierarchy().OfType<WebException>().FirstOrDefault();
            var res = exception.FlattenHierarchy().OfType<Win32Exception>().FirstOrDefault();
            if (res != null)
            {
                var innerExMessage = res.Message;
                Log($"Handling http communication exception,Found native win 32 exception {innerExMessage} and native code {res.NativeErrorCode}", logger);
                if (IsRetryableException(res.NativeErrorCode))
                {
                    Log($"Connection failure exception, can be retried");
                    webException = new WebException(innerExMessage, WebExceptionStatus.ConnectFailure);
                }
            }
            else
            {
                var httpException = exception.FlattenHierarchy().OfType<HttpRequestException>().FirstOrDefault();
                if (httpException != null)
                {
                    var innerExMessage = httpException.Message;
                    Log($"Handling http communication exception,Found http request exception {innerExMessage}", logger);
                    if (IsRetryableException(httpException.Message))
                    {
                        Log($"Connection failure exception, can be retried", logger);
                        webException = new WebException(innerExMessage, WebExceptionStatus.SendFailure);
                    }
                }
            }

            if (webException != null)
            {
                Log($"Handling http communication exception, Handling webexception of type is {webException.Status}", logger);
                switch (webException.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                    case WebExceptionStatus.NameResolutionFailure:
                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        return new HttpEndpointUnreachableException(webException.Status, exception);
                    case WebExceptionStatus.ReceiveFailure:
                    case WebExceptionStatus.SendFailure:
                    case WebExceptionStatus.ConnectionClosed:
                    case WebExceptionStatus.KeepAliveFailure:
                    case WebExceptionStatus.ServerProtocolViolation:
                        return new HttpTransportFailureException(webException.Status, exception);
                    default:
                        return new HttpCommunicationWebException(webException.Status, exception);
                }
            }

            Log($"Handling http communication exception, Falling back to HttpCommunicationException", logger);
            
            return new HttpCommunicationException(exception);
        }

        private static bool IsRetryableException(int resNativeErrorCode)
        {
            return resNativeErrorCode >= 10000 && resNativeErrorCode <= 13000;
        }

        private static bool IsRetryableException(string message)
        {
            return !string.IsNullOrEmpty(message) &&
                   message.Contains("The server returned an invalid or unrecognized response");
        }

        private static void Log(string msg, ILogger logger = null)
        {
            if (logger == null)
            {
                ServicePlatformTrace.Instance.TraceInformation(msg);
            }
            else
            {
                logger.LogInformation(msg);
            }
        }
    }
}
