[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Collation of WOPI monitored exceptions.")]

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WopiExceptions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Wopi.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Runtime.Serialization;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// WOPIClientDiscoveryException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Microsoft.D365.HCM.Common.Wopi.Exceptions", "Wopi_ClientDiscovery", MonitoredExceptionKind.Service)]
    public class WopiClientDiscoveryException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiClientDiscoveryException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public WopiClientDiscoveryException(string message)
            : base(message)
        {
        }

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

    /// <summary>
    /// WOPIInvalidAccessTokenException class
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Unauthorized, "Microsoft.D365.HCM.Common.Wopi.Exceptions", "Wopi_InvalidAccessToken", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    public class WopiInvalidAccessTokenException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiInvalidAccessTokenException" /> class.
        /// </summary>
        public WopiInvalidAccessTokenException()
            : base($"Access token is invalid!")
        {
        }

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

    /// <summary>
    /// Exception class for the proof.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Microsoft.D365.HCM.Common.Wopi.Exceptions", "Wopi_InvalidClientProof", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    public class WopiInvalidClientProof : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiInvalidClientProof" /> class.
        /// </summary>
        public WopiInvalidClientProof()
            : base("WOPI client failed validation")
        {
        }

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

    /// <summary>
    /// Exception class for unsupported WOPI operations.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.NotImplemented, "Microsoft.D365.HCM.Common.Wopi.Exceptions", "Wopi_OperationNotSupported", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    public class WopiOperationNotSupportedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiOperationNotSupportedException" /> class.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="operation">The attempted operation</param>
        public WopiOperationNotSupportedException(string fileId, string operation)
            : base($"Wopi Operation: {operation} on file with id: {fileId} is not supported!")
        {
        }

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

    /// <summary>
    /// Exception class for WOPI operations that fail the precondition check
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.PreconditionFailed, "Microsoft.D365.HCM.Common.Wopi.Exceptions", "Wopi_OperationPreconditionFailed", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    public class WopiOperationPreconditionFailedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiOperationPreconditionFailedException" /> class.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="operation">The attempted operation</param>
        public WopiOperationPreconditionFailedException(string fileId, string operation)
            : base($"Wopi Operation: {operation} on file with id: {fileId} failed the precondition check!")
        {
        }

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
