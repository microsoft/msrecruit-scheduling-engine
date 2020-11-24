//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IProofHelper.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Interfaces
{
    using Discovery;

    /// <summary>
    /// Represents a WOPI client proof validator
    /// </summary>
    public interface IProofHelper
    {
        /// <summary>
        /// Validates information coming in from a WOPI client
        /// </summary>
        /// <param name="validationPack">Contains information about the request to validate</param>
        /// <returns>True if the requests originates from the expected WOPI client</returns>
        bool Validate(WopiProofValidationPack validationPack);
    }
}
