//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IHCMPrincipalRetriever.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Security.V2
{
    public interface IHCMPrincipalRetriever
    {
        /// <summary>Gets the principal.</summary>
        IHCMPrincipal Principal { get; }
    }
}
