//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Base.Security.V2
{
    public interface IHCMPrincipalRetriever
    {
        /// <summary>Gets the principal.</summary>
        IHCMPrincipal Principal { get; }
    }
}
