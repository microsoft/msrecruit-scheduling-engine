//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp
{
    public enum UpsertBehavior
    {
        AllowCreate = 1,
        AllowUpdate = 2,
        AllowCreateOrUpdate = AllowCreate | AllowUpdate,
        AllowUpdateIfEtagMatches = 4,
    }
}
