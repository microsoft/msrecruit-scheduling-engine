//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Document for Adobe Sign Agreement.
    /// </summary>
    [DataContract]
    public class AdobeSignAgreement
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "adobesign-agreement";

        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Document Type.
        /// </summary>
        [DataMember(Name = "dtype", IsRequired = false)]
        public string DType { get; set; }

        /// <summary>
        /// Gets or sets the Tenant ID.
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the artifact ID.
        /// </summary>
        [DataMember(Name = "artifactId", IsRequired = false)]
        public string ArtifactId { get; set; }

        /// <summary>
        /// Gets or sets the artifact Type.
        /// </summary>
        [DataMember(Name = "artifactType", IsRequired = false)]
        public string ArtifactType { get; set; }

        /// <summary>
        /// Gets or sets the agreement ID.
        /// </summary>
        [DataMember(Name = "agreementId", IsRequired = false)]
        public string AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the Environment ID.
        /// </summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }
    }
}
