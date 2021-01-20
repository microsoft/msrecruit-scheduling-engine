// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferTokenDataType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTokenType
    {
        SystemToken = 0,
        PlaceholderToken = 1,
        RulesetToken = 2,
    }
}
