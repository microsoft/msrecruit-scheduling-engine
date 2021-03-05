//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using ServicePlatform.Privacy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using CommonDataService.Common.Internal;

namespace ServicePlatform.Exceptions
{
    public sealed class ServiceErrorJsonSerializer
    {
        // This comes from the monolithic serviceplatform and needs to be broken apart
        private static readonly UTF8Encoding Utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private const int ExceptionDeserializationError = 1;

        /// <summary>
        /// The logger factory is required to make this class thread-safe.
        /// </summary>        
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceErrorJsonSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory instance.</param>
        public ServiceErrorJsonSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Gets or sets a value that controls whether camel case is to be used for serialization.
        /// </summary>
        /// <remarks>Default is false.</remarks>
        public bool SerializeWithCamelCase { get; set; } = false;        

        public void Serialize(ServiceError serviceError, Stream stream)
        {
            var writer = new JsonTextWriter(new StreamWriter(stream, Utf8Encoding));

            /*
            {
              "error": {
                "namespace": "ServicePlatform", Service-Defined Namespace (ErrorNamespace on MonitoredException)
                "code": "AccessDenied",  Service-Defined Error Code (ErrorCode on MontioredException)
                "message": "Unsupported functionality",
                "@d365.details": [ Service-Defined Key-Value Pairs (Custom Data)
                  "p1": { "value": "v1", "privacy": "pii"      }, text used to Mark trace output
                  "p2": { "value": "v2"                        }, Public (by default)
                  "pN": { "value": "vN", "privacy": "internal" }
                ]
              }
            }
            */

            writer.WriteStartObject(); // wrapper

            writer.WritePropertyName("error");
            writer.WriteStartObject();
            
            if (!string.IsNullOrEmpty(serviceError.ErrorNamespace))
            {
                writer.WritePropertyName("namespace");
                writer.WriteValue(serviceError.ErrorNamespace);
            }

            writer.WritePropertyName("code");
            writer.WriteValue(serviceError.ErrorCode);

            if (serviceError.Message != null)
            {
                writer.WritePropertyName("message");
                writer.WriteValue(serviceError.Message);
            }

            if (serviceError.PropagateError != null)
            {
                writer.WritePropertyName("propagateError");
                writer.WriteValue(serviceError.PropagateError);
            }

            // Custom Data Serialization
            var serializableData = serviceError.CustomData.Values
                .Where(cd => cd.Serialize && cd.Value != null)
                .ToList();

            if (serializableData.Any())
            {
                writer.WritePropertyName("@gta.details");
                writer.WriteStartObject();

                var serializedNames = new HashSet<string>();
                foreach (var customData in serializableData)
                {
                    // First in wins
                    if (serializedNames.Add(customData.Name))
                    {
                        writer.WritePropertyName(customData.Name);
                        writer.WriteStartObject();

                        writer.WritePropertyName("value");
                        writer.WriteValue(customData.Value);

                        if (customData.PrivacyLevel != PrivacyLevel.PublicData)
                        {
                            writer.WritePropertyName("privacy");
                            writer.WriteValue(customData.PrivacyLevel);
                        }

                        //// Don't write out "serializable" it is trivally true

                        writer.WriteEndObject();
                    }
                }

                writer.WriteEndObject(); // @o365.details
            }

            writer.WriteEndObject(); // error
            writer.WriteEndObject(); // wrapper
            writer.Flush();
        }

        /// <summary>
        /// Try deserialize using the string content.
        /// </summary>
        /// <param name="content">The json error content received from HTTP response.</param>
        /// <param name="error">The service error object.</param>
        /// <returns>Returns the boolean value if the deserialization is success/ failure.</returns>
        public bool TryDeserialize(string content, out ServiceError error)
        {
            return this.TryDeserialize(() => JObject.Parse(content), out error);
        }

        /// <summary>
        /// Try deserialize using the stream content.
        /// </summary>
        /// <param name="stream">The instance of <see cref="Stream"/>.</param>
        /// <param name="error">The service error object.</param>
        /// <returns>Returns the boolean value if the deserialization is success/ failure.</returns>
        public bool TryDeserialize(Stream stream, out ServiceError error)
        {
            return this.TryDeserialize(() => JObject.Load(new JsonTextReader(new StreamReader(stream, Utf8Encoding))), out error);
        }

        private bool TryDeserialize(Func<JObject> token, out ServiceError error)
        {
            try
            {
                error = this.Deserialize(token());
                return true;
            }
            catch (Exception ex) when (ex is JsonException || ex is InvalidOperationException)
            {
                this.loggerFactory?.CreateLogger<ServiceErrorJsonSerializer>().LogWarning(
                    ExceptionDeserializationError,
                    "Failed to deserialize error response with exception: {0}",
                    ex);

                error = null;
                return false;
            }
        }

        private ServiceError Deserialize(JObject token)
        {
            var error = token["error"];
            if (error == null)
            {
                throw new InvalidOperationException($"Failed to find 'error' property in json: {token}");
            }

            var serviceNamespace = error.Value<string>("namespace") ?? string.Empty;
            var serviceCode = error.Value<string>("code") ?? string.Empty;
            var message = error.Value<string>("message") ?? string.Empty;
            var propagateError = error.Value<bool?>("propagateError");

            var details = error["@gta.details"] ?? new JObject();
            var customData = details.Children<JProperty>()
                .Select(p =>
                {
                    var pv = p.Children().First();
                    var value = pv["value"].Value<string>();

                    PrivacyLevel privacyLevel;
                    if (!Enum.TryParse<PrivacyLevel>(pv["privacy"]?.Value<string>(), out privacyLevel))
                    {
                        privacyLevel = PrivacyLevel.PublicData;
                    }

                    return new CustomData(
                        p.Name,
                        value,
                        privacyLevel,
                        serializable: true);
                });

            return new ServiceError(serviceNamespace, serviceCode, message, customData, null, propagateError);
        }
    }
}
