//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum SocialNetworkProvider
    {
        [EnumMember(Value = "linkedin")]
        LinkedIn = 0,

        [EnumMember(Value = "facebook")]
        Facebook = 1,

        [EnumMember(Value = "twitter")]
        Twitter = 2,
    }
}
