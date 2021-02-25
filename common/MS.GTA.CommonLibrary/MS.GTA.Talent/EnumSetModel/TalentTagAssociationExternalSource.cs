//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum TalentTagAssociationExternalSource
    {
        [EnumMember(Value = "internal")]
        Internal = 0,
        [EnumMember(Value = "mSDataMall")]
        MSDataMall = 1,
        [EnumMember(Value = "greenhouse")]
        Greenhouse = 2
    }
}
