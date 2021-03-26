//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TA.CommonLibrary.CommonDataService.Common.Internal;

namespace TA.CommonLibrary.ServicePlatform.Exceptions
{
    /// <summary>
    /// [De]Serialization Contract for MonitoredException data on HTTP Responses
    /// </summary>
    public sealed class ServiceError : IMonitoredError
    {
        public ServiceError(string errorNamespace, string errorCode, string message, IEnumerable<CustomData> customData)
             : this(errorNamespace, errorCode, message, customData, stackTrace: null)
        {
        }

        public ServiceError(string errorNamespace, string errorCode, string message, IEnumerable<CustomData> customData, string stackTrace)
            : this(errorNamespace, errorCode, message, customData, null, null)
        {
        }

        public ServiceError(string errorNamespace, string errorCode, string message, IEnumerable<CustomData> customData, string stackTrace, bool? propagateError)
        {
            ErrorNamespace = errorNamespace;
            ErrorCode = errorCode;
            Message = message;
            PropagateError = propagateError;

            var customDictionary = (customData ?? Enumerable.Empty<CustomData>()).ToDictionary(k => k.Name);
            CustomData = new ReadOnlyDictionary<string, CustomData>(customDictionary);
        }

        public string ErrorNamespace { get; set; }

        public string ErrorCode { get; }

        public string Message { get; }
        
        public bool? PropagateError { get; }

        public string StackTrace { get; }

        public IReadOnlyDictionary<string, CustomData> CustomData { get; }

        public override string ToString()
        {
            return
                $"Service Error details: Message {Message}, ErrorCode {ErrorCode}, ErrorNamespace: {ErrorNamespace}, StackTrace: {StackTrace}";
        }
    }
}
