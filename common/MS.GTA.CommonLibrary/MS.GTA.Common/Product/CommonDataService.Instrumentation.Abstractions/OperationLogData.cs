// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ----------------------------------------------------------------------------

using System.Text;

namespace Microsoft.CommonDataService.Instrumentation
{
    public class OperationLogData
    {
        public string OperationName { get; }
        public string ResourceId { get; }
        public string ResourceType { get; }
        public string CallerIpAddress { get; }
        public string OperationType { get; }
        public string OperationVersion { get; }
        public string ResultDescription { get; }
        public string ResultSignature { get; }
        public string ResultType { get; }
        public string TargetEndpointAddress { get; }
        public uint DurationMs { get; }
        public string CustomProperties { get; }
        public string ExceptionTypeName { get; }
        public string ExceptionCustomData { get; }

        public OperationLogData(
            string operationName,
            string resourceId,
            string resourceType,
            string callerIpAddress,
            string operationType,
            string operationVersion,
            string resultDescription,
            string resultSignature,
            string resultType,
            string targetEndpointAddress,
            uint durationMs,
            string customProperties,
            string exceptionTypeName,
            string exceptionCustomData)
        {
            OperationName = operationName;
            ResourceId = resourceId;
            ResourceType = resourceType;
            CallerIpAddress = callerIpAddress;
            OperationType = operationType;
            OperationVersion = operationVersion;
            ResultDescription = resultDescription;
            ResultSignature = resultSignature;
            ResultType = resultType;
            TargetEndpointAddress = targetEndpointAddress;
            DurationMs = durationMs;
            CustomProperties = customProperties;
            ExceptionTypeName = exceptionTypeName;
            ExceptionCustomData = exceptionCustomData;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(OperationName)}: {OperationName}");
            stringBuilder.AppendLine($"{nameof(ResourceId)}: {ResourceId}");
            stringBuilder.AppendLine($"{nameof(ResourceType)}: {ResourceType}");
            stringBuilder.AppendLine($"{nameof(CallerIpAddress)}: {CallerIpAddress}");
            stringBuilder.AppendLine($"{nameof(OperationType)}: {OperationType}");
            stringBuilder.AppendLine($"{nameof(OperationVersion)}: {OperationVersion}");
            stringBuilder.AppendLine($"{nameof(ResultDescription)}: {ResultDescription}");
            stringBuilder.AppendLine($"{nameof(ResultSignature)}: {ResultSignature}");
            stringBuilder.AppendLine($"{nameof(ResultType)}: {ResultType}");
            stringBuilder.AppendLine($"{nameof(TargetEndpointAddress)}: {TargetEndpointAddress}");
            stringBuilder.AppendLine($"{nameof(DurationMs)}: {DurationMs}");
            stringBuilder.AppendLine($"{nameof(CustomProperties)}: {CustomProperties}");
            stringBuilder.AppendLine($"{nameof(ExceptionTypeName)}: {ExceptionTypeName}");
            stringBuilder.AppendLine($"{nameof(ExceptionCustomData)}: {ExceptionCustomData}");

            return stringBuilder.ToString();
        }
    }
}
