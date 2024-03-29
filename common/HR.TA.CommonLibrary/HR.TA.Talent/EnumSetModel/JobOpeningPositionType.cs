//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobOpeningPositionType
    {
        [EnumMember(Value = "fullTime")]
        FullTime = 0,
        [EnumMember(Value = "partTime")]
        PartTime = 1,
        [EnumMember(Value = "contract")]
        Contract = 2,
        [EnumMember(Value = "temporary")]
        Temporary = 3,
        [EnumMember(Value = "volunteer")]
        Volunteer = 4
    }
}
