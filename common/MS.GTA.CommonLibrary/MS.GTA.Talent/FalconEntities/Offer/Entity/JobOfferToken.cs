﻿//----------------------------------------------------------------------------
// <copyright file="JobOfferToken.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferToken : DocDbEntity
    {

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "TokenType")]
        public JobOfferTokenType TokenType { get; set; }

        [DataMember(Name = "DataType")]
        public JobOfferTokenDataType DataType { get; set; }

        [DataMember(Name = "OriginatingTemplateId")]
        public string OriginatingTemplateId { get; set; }

        [DataMember(Name = "SectionName")]
        public string SectionName { get; set; }

        [DataMember(Name = "SectionId")]
        public string SectionId { get; set; }

        [DataMember(Name = "Templates")]
        public IList<string> Templates { get; set; }

        [DataMember(Name = "IsEditable")]
        public bool? IsEditable { get; set; }

        [DataMember(Name = "JobOfferPackageDocumentID")]
        public string JobOfferPackageDocumentID { get; set; }
    }
}
