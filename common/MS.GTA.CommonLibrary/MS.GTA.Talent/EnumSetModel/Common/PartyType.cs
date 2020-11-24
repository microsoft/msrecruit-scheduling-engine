//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="PartyType.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PartyType
    {
        Person = 0,
        Organization = 1,
        Group = 2
    }
}
