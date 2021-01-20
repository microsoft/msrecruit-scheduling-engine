//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ESignDetail.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    [DataContract]
    public class ESignDetail : DocDbEntity
    {
        [DataMember(Name = "ESignDetailID")]
        public string ESignDetailID { get; set; }

        [DataMember(Name = "OfferID")]
        public string OfferID { get; set; }

        [DataMember(Name = "ExternalDocumentID")]
        public string ExternalDocumentID { get; set; }

        [DataMember(Name = "EsignTypeSelected")]
        public ESignatureType EsignTypeSelected { get; set; }

        [DataMember(Name = "SigningUserName")]
        public string SigningUserName { get; set; }

        [DataMember(Name = "SenderObjectID")]
        public string SenderObjectID { get; set; }
    }
}
