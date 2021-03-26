//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace HR.TA.TalentEngagementService.Data
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum CandidateSocialNetworkConversationContentType
    {
        [EnumMember(Value = "mail")]
        Mail = 0,

        [EnumMember(Value = "note")]
        Note = 1,
    }
}
