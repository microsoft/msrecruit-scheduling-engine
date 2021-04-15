//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    public enum UpsertBehavior
    {
        AllowCreate = 1,
        AllowUpdate = 2,
        AllowCreateOrUpdate = AllowCreate | AllowUpdate,
        AllowUpdateIfEtagMatches = 4,
    }
}
