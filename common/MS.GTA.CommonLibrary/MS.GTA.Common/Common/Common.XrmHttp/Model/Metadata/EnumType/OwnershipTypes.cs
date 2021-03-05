//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp.Model.Metadata
{
    using System;

    [Flags]
    public enum OwnershipTypes
    {
        None = 0,
        UserOwned = 1,
        TeamOwned = 2,
        BusinessOwned = 4,
        OrganizationOwned = 8,
        BusinessParented = 16,
    }
}
