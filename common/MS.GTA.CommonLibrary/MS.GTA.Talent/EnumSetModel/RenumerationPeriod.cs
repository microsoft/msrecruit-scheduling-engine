//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum RenumerationPeriod
    {
        [EnumMember(Value = "weekly")]
        Weekly = 0,

        [EnumMember(Value = "biWeekly")]
        ByWeekly = 1,

        [EnumMember(Value = "Monthly")]
        Monthly = 2,
    }
}
