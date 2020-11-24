//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateSocialNetworkConversationContentType.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEngagementService.Data
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum CandidateSocialNetworkConversationContentType
    {
        [EnumMember(Value = "mail")]
        Mail = 0,

        [EnumMember(Value = "note")]
        Note = 1,
    }
}
