//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="systemuser.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model
{
    public enum SystemUserAccessMode
    {
        ReadWrite,
        Administrative,
        Read,
        SupportUser,
        NonInteractive,
        DelegatedAdmin,
    }
}
