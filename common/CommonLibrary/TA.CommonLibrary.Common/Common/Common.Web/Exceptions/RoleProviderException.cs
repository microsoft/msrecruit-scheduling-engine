//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Web.Exceptions
{
    using TA.CommonLibrary.Common.Base;
    using TA.CommonLibrary.Common.Base.Exceptions;
    using TA.CommonLibrary.ServicePlatform.Exceptions;
    using TA.CommonLibrary.ServicePlatform.Privacy;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Exception thrown based on role provider service error.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "TA.CommonLibrary.Common.Web", "RolePorviderRemoteServiceException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class RoleProviderRemoteServiceException : BenignException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProviderRemoteServiceException" /> class.
        /// </summary>
        /// <param name="absoluteUri">Absolute uri to role provider service.</param>
        /// <param name="statusCode">Status code.</param>
        /// <param name="errorMessage">Error message.</param>
        public RoleProviderRemoteServiceException(string absoluteUri, string statusCode, string errorMessage) : base($"Exception during {absoluteUri} call for admin. Response {statusCode} with error message {errorMessage}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProviderRemoteServiceException" /> class.
        /// </summary>
        /// <param name="absoluteUri">Absolute uri to role provider service.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="statusCode">Status code.</param>
        public RoleProviderRemoteServiceException(string absoluteUri)
            : base($"Exception during {absoluteUri} call for admin, with null response.")
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Transient;
    }

    /// <summary>
    /// Exception thrown based on role provider permission issue.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "TA.CommonLibrary.Common.Web", "RoleProviderUnauthorizedException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class RoleProviderException : BenignException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProviderRemoteServiceException" /> class.
        /// </summary>        
        /// <param name="errorMessage">Error message.</param>
        public RoleProviderException(string errorMessage) : base(errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets error message.
        /// </summary>
        [ExceptionCustomData(
            Name = "ErrorMessage",
            Serialize = true)]
        public string ErrorMessage { get; }
    }
}
