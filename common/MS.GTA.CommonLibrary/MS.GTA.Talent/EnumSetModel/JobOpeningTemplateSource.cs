//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningTemplateSource
    {
        [EnumMember(Value = "defaultApplication")]
        DefaultApplication = 0,
        [EnumMember(Value = "application")]
        Application = 1
    }
}
