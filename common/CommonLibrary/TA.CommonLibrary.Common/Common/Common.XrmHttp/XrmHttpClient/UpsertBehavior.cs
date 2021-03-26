//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    public enum UpsertBehavior
    {
        AllowCreate = 1,
        AllowUpdate = 2,
        AllowCreateOrUpdate = AllowCreate | AllowUpdate,
        AllowUpdateIfEtagMatches = 4,
    }
}
