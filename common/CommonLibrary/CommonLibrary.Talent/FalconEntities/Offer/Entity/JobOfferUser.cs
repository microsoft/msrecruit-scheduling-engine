//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Talent.FalconEntities.Offer.Entity
{
    using CommonLibrary.Common.DocumentDB.Contracts;
    using CommonLibrary.Common.OfferManagement.Contracts.V2;
    using CommonLibrary.Common.Web.Contracts;
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
