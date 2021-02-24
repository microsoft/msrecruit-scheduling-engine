//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Web.S2SHandler.Exceptions
{
    using System;
    using System.Net;

    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>The invalid context exception.</summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "Microsoft.D365.HCM.Common.S2S", "InvalidContextException", MonitoredExceptionKind.Service)]
    public class InvalidContextException : MonitoredException
    {
        /// <summary>Initializes a new instance of the <see cref="InvalidContextException"/> class.</summary>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="requestedType">The requested type.</param>
        public InvalidContextException(Type expectedType, Type requestedType)
            : base($"Unable to process request as request was expected to be made in {GetTypeString(expectedType)} but request was made in {GetTypeString(requestedType)}")
        {
        }

        /// <summary>The create invalid context exception.</summary>
        /// <param name="requestedType">The requested Type.</param>
        /// <typeparam name="ExpectedType">The type that we should indicate we expected</typeparam>
        /// <returns>The <see cref="InvalidContextException"/>The exception to throw.</returns>
        public static InvalidContextException CreateInvalidContextException<ExpectedType>(Type requestedType)
            where ExpectedType : IHCMPrincipal
        {
            return new InvalidContextException(typeof(ExpectedType), requestedType);
        }

        /// <summary>The create invalid context exception.</summary>
        /// <typeparam name="ExpectedType">The type that we should indicate we expected.</typeparam>
        /// <typeparam name="RequestedType">The type we should indicate we requested for.</typeparam>
        /// <returns>The <see cref="InvalidContextException"/>The exception to throw.</returns>
        public static InvalidContextException CreateInvalidContextException<ExpectedType, RequestedType>()
            where ExpectedType : IHCMPrincipal
            where RequestedType : IHCMPrincipal
        {
            return new InvalidContextException(typeof(ExpectedType), typeof(RequestedType));
        }

        /// <summary>The get type string.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetTypeString(Type type)
        {
            if (type is IHCMUserPrincipal)
            {
                return "user";
            }

            if (type is IHCMB2CPrincipal)
            {
                return "B2C";
            }

            if (type is IHCMB2BPrincipal)
            {
                return "B2B";
            }

            if (type is IHCMApplicationPrincipal)
            {
                return "application";
            }

            return "unknown";
        }
    }

}
