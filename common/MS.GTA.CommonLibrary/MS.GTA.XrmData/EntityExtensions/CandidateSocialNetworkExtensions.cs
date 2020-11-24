//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateSocialNetworkExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class CandidateSocialNetworkExtensions
    {
        public static SocialIdentity ToViewModel(this CandidateSocialNetwork candidateSocialNetwork) => candidateSocialNetwork == null ? null : new SocialIdentity()
        {
            Provider = candidateSocialNetwork.SocialNetwork.Value.ToSocialNetworkProvider(),
            ProviderMemberId = candidateSocialNetwork.MemberReference
        };

        public static SocialNetworkProvider ToSocialNetworkProvider(this SocialNetwork socialNetwork)
        {
            switch (socialNetwork)
            {
                case SocialNetwork.Facebook:
                    return SocialNetworkProvider.Facebook;
                case SocialNetwork.Twitter:
                    return SocialNetworkProvider.Twitter;
                case SocialNetwork.LinkedIn:
                default:
                    return SocialNetworkProvider.LinkedIn;
            }
        }
    }
}
