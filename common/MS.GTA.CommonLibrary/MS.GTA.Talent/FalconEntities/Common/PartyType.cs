//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="PartyType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentEntities.Enum.Common
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