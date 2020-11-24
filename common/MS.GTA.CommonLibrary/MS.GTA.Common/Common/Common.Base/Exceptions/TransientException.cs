//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TransientException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Base.Exceptions
{
    using MS.GTA.Common.Base;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Privacy;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Transient Exception class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.Base", "TransientException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public class TransientException : MonitoredException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public TransientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="exception">The exception.</param>
        public TransientException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Transient;
    }
}
