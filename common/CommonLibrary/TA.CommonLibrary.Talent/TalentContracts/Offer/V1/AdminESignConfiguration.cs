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
    /// Document for Admin ESign configuration.
    /// </summary>
    [DataContract]
    public class AdminESignConfiguration
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "esign-admin";

        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Document Type.
        /// </summary>
        [DataMember(Name = "esignTypeSelected", IsRequired = false)]
        public ESignType ESignTypeSelected { get; set; }

        /// <summary>
        /// Gets or sets the Tenant ID.
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets environmentId for the tenant.
        /// </summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }
    }
}
