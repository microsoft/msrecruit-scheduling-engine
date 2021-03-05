//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient.Contracts
{
    using System.Collections.Generic;

    /// <summary>The permission collection.</summary>
    public class PermissionCollection
    {
        /// <summary>Gets or sets the access permissions.</summary>        
        public IEnumerable<Permission> AccessPermissions { get; set; }
    }
}
