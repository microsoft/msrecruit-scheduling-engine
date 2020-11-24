using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions.Cache;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Provides the default implementation for <see cref="IServiceExceptionHandlingProvider"/>.
    /// </summary>
    /// <remarks>If the exception is annotated with <see cref="MonitoredExceptionMetadataAttribute"/> will use that attribute to generate
    /// and serialize a <see cref="ServiceError"/>. If that attribute is not provided, it will generate and serialize a <see cref="ServiceError"/>
    /// portraing an unknown error.</remarks>
    public class DefaultServiceExceptionHandlingProvider : IServiceExceptionHandlingProvider
    {
        private readonly ServiceErrorJsonSerializer serializer;

        /// <summary>
        /// ILogger is not thread-safe, thus we need to create a new logger on each call.
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServiceExceptionHandlingProvider"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public DefaultServiceExceptionHandlingProvider(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;
            this.serializer = new ServiceErrorJsonSerializer(loggerFactory);
        }

        /// <summary>
        /// Gets or sets a value defining whether camel case should be used for serialization.
        /// </summary>
        /// <remarks>This is applicable to custom properties defined through <see cref="ExceptionCustomDataAttribute"/>. Default is <see cref="false"/>.</remarks>
        public bool SerializeWithCamelCase { get; set; } = false;

        /// <summary>
        /// Determines whether the provided <paramref name="exception"/> should be considered fatal.
        /// </summary>
        /// <remarks>
        /// An exception is considered as Fatal if it is defined as such using <see cref="MonitoredExceptionMetadataAttribute"/>.
        /// </remarks>
        public virtual bool IsExceptionFatal(Exception exception)
        {
            Contract.CheckValue(exception, nameof(exception));
            var cacheEntry = ExceptionTypeCache.GetCacheEntry(exception.GetType());
            return cacheEntry.Metadata.Kind == MonitoredExceptionKind.Fatal;
        }

        /// <summary>
        /// Gets an HTTP status code for the provided <paramref name="exception"/>.
        /// </summary>
        public virtual int GetHttpStatusCode(Exception exception)
        {
            Contract.CheckValue(exception, nameof(exception));
            var cacheEntry = ExceptionTypeCache.GetCacheEntry(exception.GetType());
            return cacheEntry.Metadata.HttpStatusCode;
        }

        /// <summary>
        /// Serializes the <paramref name="exception"/> into the <paramref name="stream"/>.
        /// </summary>
        /// <param name="exception">The exception to be serialized.</param>
        /// <param name="stream">The stream to receive the serialized exception.</param>
        public virtual void Serialize(Exception exception, Stream stream)
        {
            Contract.CheckValue(exception, nameof(exception));
            ServiceError error = this.CreateServiceError(exception);
            this.serializer.Serialize(error, stream);
        }

        /// <summary>
        /// Creates an instance of <see cref="ServiceError"/> out of the <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception to be converted to service error.</param>
        /// <returns>The service error representing the exception.</returns>
        protected virtual ServiceError CreateServiceError(Exception exception)
        {
            var cacheEntry = ExceptionTypeCache.GetCacheEntry(exception.GetType());

            var customData = cacheEntry.CustomDataPropertyReferences
                .Where(d => d.Serialize)
                .Select(d => this.ToCustomData(d, exception));

            return new ServiceError(
                cacheEntry.Metadata.ErrorNamespace,
                cacheEntry.Metadata.ServiceErrorCode,
                exception.Message, 
                customData,
                null,
                cacheEntry.Metadata.PropagateError);
        }

        private CustomData ToCustomData(CustomDataPropertyReference reference, Exception exception)
        {
            string name = this.SerializeWithCamelCase
                ? ToCamelCase(reference.Name)
                : reference.Name;

            return new CustomData(
                name,
                reference.GetValue(exception)?.ToString(),
                reference.PrivacyLevel,
                reference.Serialize);
        }

        private static string ToCamelCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !char.IsUpper(name[0]))
            {
                return name;
            }

            char first = char.ToLower(name[0]);
            return first + name.Substring(1);
        }
    }
}
