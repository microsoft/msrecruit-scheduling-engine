//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Privacy;
using MS.GTA.ServicePlatform.Exceptions.Cache;
using MS.GTA.CommonDataService.Instrumentation.Privacy;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Base class for all service exceptions.
    /// </summary>
    [Serializable]
    public abstract partial class MonitoredException : Exception
    {
        private readonly ExceptionCacheEntry cacheEntry;
        private int traceState;

        public MonitoredException()
            : this(message: null, innerException: null)
        {
        }

        public MonitoredException(string message)
            : this(message, innerException: null)
        {
        }

        public MonitoredException(string message, Exception innerException)
            : this(message: message, serviceError: null, innerException: innerException)
        {
        }

        public MonitoredException(string message, ServiceError serviceError, Exception innerException)
            : base(message, innerException)
        {
            this.RemoteServiceError = serviceError;

            cacheEntry = ExceptionTypeCache.GetCacheEntry(this.GetType());
        }

        protected MonitoredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Nothing to serialize, here to allow derived classes to correctly implement ISerializable
            cacheEntry = ExceptionTypeCache.GetCacheEntry(this.GetType());
        }

        /// <summary>
        /// Gets the HTTP Status Code
        /// </summary>
        public virtual int HttpStatus
        {
            get { return cacheEntry.Metadata.HttpStatusCode; }
        }

        /// <summary>
        /// Gets the service error information.
        /// </summary>
        public ServiceError RemoteServiceError { get; }

        /// <summary>
        /// Gets the Service-Defined Error Namespace
        /// </summary>
        public virtual string ErrorNamespace
        {
            get { return cacheEntry.Metadata.ErrorNamespace; }
        }

        /// <summary>
        /// Gets the Service-Defined Error Code
        /// </summary>
        public virtual string ServiceErrorCode
        {
            get { return cacheEntry.Metadata.ServiceErrorCode; }
        }

        /// <summary>
        /// Gets the <see cref="MonitoredExceptionKind"/> of the Exception. <seealso cref="MonitoredExceptionMetadataAttribute"/>
        /// </summary>
        public virtual MonitoredExceptionKind Kind
        {
            get { return cacheEntry.Metadata.Kind; }
        }

        /// <summary>
        /// Trace (once and only once) to <see cref="ServicePlatformTrace"/>. 
        /// </summary>
        /// <returns>The current <see cref="MonitoredException"/>.</returns>
        internal bool MarkTraced() => Interlocked.CompareExchange(ref traceState, 1, 0) == 0;

        public override string ToString()
        {
            var customDataText = string.Join(
                Environment.NewLine,
                GetCustomData().Select(cd => $"\t{cd.Name} = {cd.MarkedValue}"));

            var innerExceptionText = InnerException != null
                ? string.Concat("---> ", InnerException.ToString(), Environment.NewLine, "<--- End Inner Exception")
                : null;

            return $"{cacheEntry.TypeName}: {Message}{Environment.NewLine}{customDataText}{Environment.NewLine}{innerExceptionText}{Environment.NewLine}{StackTrace}";
        }

        internal virtual IEnumerable<CustomData> GetCustomData(bool forSerialization = false)
        {
            var data = cacheEntry.CustomDataPropertyReferences as IEnumerable<CustomDataPropertyReference>;
            if (forSerialization)
            {
                data = data.Where(d => d.Serialize);
            }

            return data.Select(d => d.Bind(this));
        }
    }
}
