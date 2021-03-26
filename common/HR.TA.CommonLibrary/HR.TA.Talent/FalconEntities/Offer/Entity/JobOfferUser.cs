//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Talent.FalconEntities.Offer.Entity
{
    using HR.TA.Common.DocumentDB.Contracts;
    using HR.TA.Common.OfferManagement.Contracts.V2;
    using HR.TA.Common.Web.Contracts;
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
