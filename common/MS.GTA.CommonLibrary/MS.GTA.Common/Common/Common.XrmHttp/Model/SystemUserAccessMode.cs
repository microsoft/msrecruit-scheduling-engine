//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="systemuser.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
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
