//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    public enum UpsertBehavior
    {
        AllowCreate = 1,
        AllowUpdate = 2,
        AllowCreateOrUpdate = AllowCreate | AllowUpdate,
        AllowUpdateIfEtagMatches = 4,
    }
}
