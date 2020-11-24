//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="UserESignConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Document for User ESign configuration.
    /// </summary>
    [DataContract]
    public class UserESignConfiguration
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "esign-user";

        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user oid.
        /// </summary>
        [DataMember(Name = "oid", IsRequired = false)]
        public string OID { get; set; }

        /// <summary>
        /// Gets or sets the user refresh token.
        /// </summary>
        [DataMember(Name = "refreshToken", IsRequired = false)]
        public string RefreshToken { get; set; }

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

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [DataMember(Name = "emailAddress", IsRequired = false)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the api access point.
        /// </summary>
        /// <value>
        /// The api access point.
        /// </value>
        [DataMember(Name = "apiAccessPoint", IsRequired = false)]
        public string ApiAccessPoint { get; set; }

        /// <summary>
        /// Gets or sets the web access point.
        /// </summary>
        /// <value>
        /// The web access point.
        /// </value>
        [DataMember(Name = "webAccessPoint", IsRequired = false)]
        public string WebAccessPoint { get; set; }
    }
}
