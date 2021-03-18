//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
