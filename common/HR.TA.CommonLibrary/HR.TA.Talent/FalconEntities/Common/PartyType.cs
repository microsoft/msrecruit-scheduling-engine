﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentEntities.Enum.Common
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