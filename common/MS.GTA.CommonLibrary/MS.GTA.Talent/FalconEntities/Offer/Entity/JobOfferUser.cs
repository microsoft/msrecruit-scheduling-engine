//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Talent.FalconEntities.Offer.Entity
{
    using Common.DocumentDB.Contracts;
    using Common.OfferManagement.Contracts.V2;
    using Common.Web.Contracts;
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
