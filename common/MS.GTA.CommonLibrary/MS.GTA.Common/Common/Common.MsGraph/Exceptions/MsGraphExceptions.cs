[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Small closely related classes may be combined.")]

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="MsGraphExceptions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.MSGraph.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using ServicePlatform.Exceptions;
    using System.Net;

    /// <summary>
    /// GraphServiceException class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Small closely related classes may be combined.")]
    [Serializable]
    public partial class GraphServiceException : MonitoredException
    {
        /// <summary>
        /// Message string
        /// </summary>
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the<see cref="GraphServiceException" /> class.
        /// </summary>
        /// <param name='message'>The message that explains the reason for the exception.</param>
        public GraphServiceException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceException" /> class.
        /// </summary>
        /// <param name='message'>The message that explains the reason for the exception.</param>
        /// <param name='innerException'>The exception that is the cause of the current exception.
        /// If it is not a null reference, the the current exception is raised in a catch block
        /// that handles the inner exception.</param>
        public GraphServiceException(string message, Exception innerException) : base(message, innerException)
        {
            this.message = message;
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
    /// Graph service unavailable propagation exception. 
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.ServiceUnavailable, "MS.GTA.Common.MsGraph", "MsGraphServiceUnavailableException", MonitoredExceptionKind.Service)]
    public sealed class MsGraphServiceUnavailableException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MS.GTA.Common.MSGraph.Exceptions.MsGraphServiceUnavailableException" /> class.
        /// </summary>
        /// <param name="exception">Exception during graph call.</param>
        public MsGraphServiceUnavailableException(Exception exception) : base($"Issue while thrown by graph sdk, ErrorMessage: {exception.Message}, stacktrace:{exception.StackTrace}")
        {
        }
    }

    /// <summary>
    /// Graph bad request propagation exception. 
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.Common.MsGraph", "MsGraphInvalidInputException", MonitoredExceptionKind.Service)]
    public sealed class MsGraphInvalidInputException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MS.GTA.Common.MSGraph.Exceptions.MsGraphInvalidInputException" /> class.
        /// </summary>
        /// <param name="exception">Exception during graph call.</param>
        public MsGraphInvalidInputException(Exception exception) : base($"Issue while thrown by graph sdk, ErrorMessage: {exception.Message}, stacktrace:{exception.StackTrace}")
        {
        }
    }

    /// <summary>
    /// AzureActiveDirectoryClientException class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Small closely related classes may be combined.")]
    [Serializable]
    internal partial class AzureActiveDirectoryClientException : MonitoredException
    {
        /// <summary>
        /// Message string
        /// </summary>
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureActiveDirectoryClientException" /> class.
        /// </summary>
        /// <param name='message'>The message that explains the reason for the exception.</param>
        public AzureActiveDirectoryClientException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureActiveDirectoryClientException" /> class.
        /// </summary>
        /// <param name='message'>The message that explains the reason for the exception.</param>
        /// <param name='innerException'>The exception that is the cause of the current exception.
        /// If it is not a null reference, the the current exception is raised in a catch block
        /// that handles the inner exception.</param>
        public AzureActiveDirectoryClientException(string message, Exception innerException) : base(message)
        {
            this.message = message;
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
    /// Scheduler service token exception. 
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.MsGraph", "AcquireGraphTokenException", MonitoredExceptionKind.Service)]
    internal sealed class AcquireGraphTokenException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcquireGraphTokenException" /> class.
        /// </summary>
        /// <param name="exception">Exception during scheduling service token acquisition.</param>
        internal AcquireGraphTokenException(Exception exception) : base($"Issue while acquiring token, Exception:{exception}")
        {
        }
    }
}