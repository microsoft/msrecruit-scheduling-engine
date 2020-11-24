//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="PermissionCollection.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts
{
    using System.Collections.Generic;

    /// <summary>The permission collection.</summary>
    public class PermissionCollection
    {
        /// <summary>Gets or sets the access permissions.</summary>        
        public IEnumerable<Permission> AccessPermissions { get; set; }
    }
}
