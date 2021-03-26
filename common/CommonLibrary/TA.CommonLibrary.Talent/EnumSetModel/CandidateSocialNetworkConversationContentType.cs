//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEngagementService.Data
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum CandidateSocialNetworkConversationContentType
    {
        [EnumMember(Value = "mail")]
        Mail = 0,

        [EnumMember(Value = "note")]
        Note = 1,
    }
}
