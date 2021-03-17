//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using CommonLibrary.Common.DocumentDB.Contracts;

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