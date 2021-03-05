//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobApplicationExternalSource
    {
        [EnumMember(Value = "internal")]
        Internal = 0,
        [EnumMember(Value = "mSDataMall")]
        MSDataMall = 1,
        [EnumMember(Value = "linkedIn")]
        LinkedIn = 2,
        [EnumMember(Value = "greenhouse")]
        Greenhouse = 3,
        [EnumMember(Value = "iCIMS")]
        ICIMS = 4
    }
}
