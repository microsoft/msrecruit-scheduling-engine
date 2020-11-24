//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="MSIntToken.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Integration.Contracts
{
    /// <summary>
    /// MSIntToken
    /// </summary>
    public class MSIntToken
    {
        public string TokenName { get; set; }

        public string Value { get; set; }

        public string DefaultValue { get; set; }

        public string TokenType { get; set; }

        public bool? IsOverridden { get; set; }
    }
}