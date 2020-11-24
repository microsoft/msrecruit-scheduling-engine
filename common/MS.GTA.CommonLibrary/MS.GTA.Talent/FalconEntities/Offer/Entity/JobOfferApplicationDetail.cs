//----------------------------------------------------------------------------
// <copyright file="JobOfferApplicationDetail.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferApplicationDetail : DocDbEntity
    {
        [DataMember(Name = "JobApplicationId")]
        public string JobApplicationId { get; set; }

        [DataMember(Name = "JobOpeningId")]
        public string JobOpeningId { get; set; }

        [DataMember(Name = "Status")]
        public JobOfferApplicationStatus Status { get; set; }

        [DataMember(Name = "JobOfferId")]
        public string JobOfferId { get; set; }        
    }
}
