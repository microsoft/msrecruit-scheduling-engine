//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobOpeningTemplateSource
    {
        [EnumMember(Value = "defaultApplication")]
        DefaultApplication = 0,
        [EnumMember(Value = "application")]
        Application = 1
    }
}
