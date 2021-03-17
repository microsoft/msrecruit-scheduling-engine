//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
