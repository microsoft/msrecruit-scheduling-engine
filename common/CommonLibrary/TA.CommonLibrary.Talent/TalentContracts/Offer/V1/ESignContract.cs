//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using TA.CommonLibrary.Common.TalentAttract.Contract;

    /// <summary>
    /// Document for ESign Offer.
    /// </summary>
    [DataContract]
    public class ESignContract
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "esign-id";

        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ESign Type Selected.
        /// </summary>
        [DataMember(Name = "esignTypeSelected", IsRequired = false)]
        public ESignType ESignTypeSelected { get; set; }

        /// <summary>
        /// Gets or sets the Artifact ID.
        /// </summary>
        [DataMember(Name = "artifactId", IsRequired = false)]
        public string ArtifactId { get; set; }

        /// <summary>
        /// Gets or sets the Artifact Type.
        /// </summary>
        [DataMember(Name = "artifactType", IsRequired = false)]
        public string ArtifactType { get; set; }

        /// <summary>
        /// Gets or sets the Offer ID.
        /// </summary>
        [DataMember(Name = "entityType", IsRequired = false)]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the Envelope ID.
        /// </summary>
        [DataMember(Name = "envelopeId", IsRequired = false)]
        public string EnvelopeId { get; set; }

        /// <summary>
        /// Gets or sets the Tenant ID.
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the Environment ID.
        /// </summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the Environment ID.
        /// </summary>
        [DataMember(Name = "candidateName", IsRequired = false)]
        public string CandidateName { get; set; }

        /// <summary>
        /// Gets or sets the user oid.
        /// </summary>
        [DataMember(Name = "oid", IsRequired = false)]
        public string OID { get; set; }
    }
}
