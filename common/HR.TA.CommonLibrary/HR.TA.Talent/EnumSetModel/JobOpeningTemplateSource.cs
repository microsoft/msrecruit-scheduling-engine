//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobOpeningTemplateSource
    {
        [EnumMember(Value = "defaultApplication")]
        DefaultApplication = 0,
        [EnumMember(Value = "application")]
        Application = 1
    }
}
