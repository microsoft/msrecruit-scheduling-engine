//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Talent.FalconEntities.Offer.Entity
{
    using TA.CommonLibrary.Common.DocumentDB.Contracts;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V2;
    using TA.CommonLibrary.Common.Web.Contracts;
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
