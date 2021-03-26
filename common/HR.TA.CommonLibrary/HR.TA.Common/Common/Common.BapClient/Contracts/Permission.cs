//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.BapClient.Contracts
{
    /// <summary>The permission.</summary>
    public class Permission
    {
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the display name.</summary>
        public string DisplayName { get; set; }

        /// <summary>Gets or sets the has access.</summary>
        public bool HasAccess { get; set; }

        /// <summary>Gets or sets the error code.</summary>
        public string ErrorCode { get; set; }        
    }
}
