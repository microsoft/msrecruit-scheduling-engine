//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace TA.CommonLibrary.Common.Base.Exceptions
{
    using TA.CommonLibrary.ServicePlatform.Exceptions;
    using TA.CommonLibrary.ServicePlatform.Privacy;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;

    /// <summary>
    /// Graph call failed because the application principal is not provisioned.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Unauthorized, "TA.CommonLibrary.Common.Base.Exceptions", nameof(ApplicationPrincipalNotProvisionedException), MonitoredExceptionKind.Benign)]
    public sealed class ApplicationPrincipalNotProvisionedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPrincipalNotProvisionedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ApplicationPrincipalNotProvisionedException(string message)
            : base(message)
        {
            this.ErrorMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPrincipalNotProvisionedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ApplicationPrincipalNotProvisionedException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorMessage = message;
        }

        [ExceptionCustomData(Name = Constants.ExceptionMessageTypeName, PrivacyLevel = PrivacyLevel.CustomerData, Serialize = true)]
        public string ErrorMessage { get; private set; }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Transient;
    }
}
