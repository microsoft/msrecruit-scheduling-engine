//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.Common.Base.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Net;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// Data validation exception.
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "HR.TA.Common.Base.Exceptions", "DataValidationException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class DataValidationException : BenignException
    {
        /// <summary>Initializes a new instance of the <see cref="DataValidationException"/> class.</summary>
        /// <param name="propertyName">The validation property field.</param>
        /// <param name="rule">The validation rule.</param>
        public DataValidationException(string propertyName, string rule) : base($"Data validation failed for property: {propertyName} due to reason: {rule}.")
        {
            this.PropertyName = propertyName;
            this.Rule = rule;
        }

        /// <summary>
        /// Gets the propertyName.
        /// </summary>
        [ExceptionCustomData(Name = "propertyName", Serialize = true)]
        public string PropertyName { get; }

        /// <summary>
        /// Gets the rule.
        /// </summary>
        [ExceptionCustomData(Name = "rule", Serialize = true)]
        public string Rule { get; }

        /// <summary>
        /// GetObjectData calls into GetObjectData of Serializable
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
