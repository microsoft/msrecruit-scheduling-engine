//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base.Security.V2
{
    using MS.GTA.ServicePlatform.Security;

    /// <summary>The HCMPrincipal interface.</summary>
    public interface IHCMPrincipal : IServiceContextPrincipal
    {
        /// <summary>
        /// Gets The audience.
        /// </summary>
        string Audience { get; }

        /// <summary>
        /// Gets the issuer identifier
        /// </summary>
        string Issuer { get; }

        /// <summary>Gets the token from the request headers.</summary>
        string Token { get; }
    }
}
