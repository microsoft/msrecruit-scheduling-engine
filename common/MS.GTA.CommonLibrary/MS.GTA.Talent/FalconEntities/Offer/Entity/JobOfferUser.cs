//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferUser.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Talent.FalconEntities.Offer.Entity
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.OfferManagement.Contracts.V2;
    using MS.GTA.Common.Web.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferUser : DocDbEntity
    {
        [DataMember(Name = "Person")]
        public Person Person { get; set; }

        [DataMember(Name = "Roles")]
        public IList<OfferApplicationRole> Roles { get; set; }
    }
}
