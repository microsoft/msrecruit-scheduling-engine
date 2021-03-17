//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferTemplateSection
    {
        [DataMember(Name = "SectionName")]
        public string SectionName { get; set; }

        [DataMember(Name = "SectionColor")]
        public string SectionColor { get; set; }

        [DataMember(Name ="Tokens")]
        public IList<JobOfferToken> Tokens { get; set; }
    }
}
