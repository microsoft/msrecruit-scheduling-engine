//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TalentEngagementService.Data
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateSocialNetworkConversationContentType
    {
        [EnumMember(Value = "mail")]
        Mail = 0,

        [EnumMember(Value = "note")]
        Note = 1,
    }
}
