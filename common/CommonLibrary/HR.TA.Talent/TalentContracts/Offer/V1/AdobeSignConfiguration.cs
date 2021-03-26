//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Document for Adobe Sign configuration.
    /// </summary>
    [DataContract]
    public class AdobeSignConfiguration
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "adobesign-connection";

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
        /// Gets or sets the Environment ID.
        /// </summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the Adobe Sign API Application Client ID.
        /// </summary>
        [DataMember(Name = "clientId", IsRequired = false)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Adobe Sign API Application Client Secret.
        /// </summary>
        [DataMember(Name = "clientSecret", IsRequired = false)]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Adobe Sign Refresh Token.
        /// </summary>
        [DataMember(Name = "refreshToken", IsRequired = false)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Adobe Sign is enabled.
        /// </summary>
        [DataMember(Name = "isAdobeSignEnabled", IsRequired = false)]
        public bool IsAdobeSignEnabled { get; set; }
    }
}
