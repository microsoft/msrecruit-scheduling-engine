//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SystemUserExtension.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Model;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class SystemUserExtension
    {
        public static Person ToViewModel(this SystemUser user) => user?.IsSyncWithDirectory != true ? null : new Person()
        {
            ObjectId = user.AzureActiveDirectoryObjectId.Value.ToString(),
            FullName = user.FullName,
            GivenName = user.LastName,
            MiddleName = user.MiddleName,
            Surname = user.FirstName,
            Email = user.PrimaryEmail,
        };
    }
}
