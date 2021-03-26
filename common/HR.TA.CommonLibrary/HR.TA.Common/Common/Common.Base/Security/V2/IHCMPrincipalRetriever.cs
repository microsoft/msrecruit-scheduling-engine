//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Base.Security.V2
{
    public interface IHCMPrincipalRetriever
    {
        /// <summary>Gets the principal.</summary>
        IHCMPrincipal Principal { get; }
    }
}