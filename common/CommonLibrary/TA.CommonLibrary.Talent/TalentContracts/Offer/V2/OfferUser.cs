//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Talent.TalentContracts.Offer.V2
{
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V2;
    using TA.CommonLibrary.Common.Web.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;


    /// <summary>
    /// Offer user data contract
    /// </summary>
    [DataContract]
    public class OfferUser
    {
        [DataMember(Name = "person", IsRequired = true)]
        public Person Person { get; set; }

        [DataMember(Name = "roles", IsRequired = true)]
        public IList<OfferApplicationRole> Roles { get; set; }
    }
}
