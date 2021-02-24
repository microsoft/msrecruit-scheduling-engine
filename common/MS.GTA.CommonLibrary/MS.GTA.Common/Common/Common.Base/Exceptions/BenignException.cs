//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using MS.GTA.Common.Base;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Privacy;

    /// <summary>
    /// Benign exception class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    public class BenignException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BenignException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BenignException(string message)
              : base(message)
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }
}
