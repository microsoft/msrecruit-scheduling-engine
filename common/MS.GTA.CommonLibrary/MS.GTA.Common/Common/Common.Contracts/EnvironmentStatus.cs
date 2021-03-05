//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>The environment status.</summary>
    [DataContract]
    public class EnvironmentStatus
    {
        /// <summary>
        /// Gets or sets the bap location.
        /// </summary>
        [DataMember(Name = "bapLocation")]
        public string BapLocation { get; set; }

        /// <summary>
        /// Gets or sets the current tenant's cluster URI
        /// </summary>
        [DataMember(Name = "clusterUri")]
        public string ClusterUri { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the environment status code.
        /// </summary>
        [DataMember(Name = "environmentStatusCode")]
        public EnvironmentStatusCode EnvironmentStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the BAP environment id
        /// </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether previous tenant taken over by current tenant
        /// </summary>
        [DataMember(Name = "isTenantTakenOver")]
        public bool IsTenantTakenOver { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether relevance search org setting is enabled on the environment's first user login
        /// </summary>
        [DataMember(Name = "isRelSearchEnabledOnFirstLogin", IsRequired = false)]
        public bool IsRelSearchEnabledOnFirstLogin { get; set; }

        /// <summary>
        /// Gets or sets provisioned CDS Namespace's ID.
        /// </summary>
        [DataMember(Name = "namespaceId")]
        public string NamespaceID { get; set; }

        /// <summary>
        /// Gets or sets provisioned CDS Namespace's Runtime Uri.
        /// </summary>
        [DataMember(Name = "namespaceRuntimeUri")]
        public string NamespaceRuntimeUri { get; set; }

        /// <summary>
        /// Gets or sets the environment package statuses.
        /// </summary>
        [DataMember(Name = "packageStatuses")]
        public IList<PackageStatus> PackageStatuses { get; set; } = new List<PackageStatus>();

        /// <summary>
        /// Gets or sets the previous tenant id
        /// </summary>
        [DataMember(Name = "previousTenantId")]
        public string PreviousTenantID { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the current tenant id
        /// </summary>
        [DataMember(Name = "tenantId")]
        public string TenantID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the environment is a test drive environment.
        /// </summary>
        [DataMember(Name = "testDrive")]
        public bool TestDrive { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        [DataMember(Name = "errorDetails")]
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the deletion details.
        /// </summary>
        [DataMember(Name = "deletionDetails")]
        public string DeletionDetails { get; set; }

        /// <summary>Gets or sets the environments mode.</summary>
        [DataMember(Name = "environmentMode")]
        public EnvironmentMode Mode { get; set; }

        /// <summary>Gets or sets the falcon database id.</summary>
        [DataMember(Name = "falconDatabaseId", IsRequired = false)]
        public string FalconDatabaseId { get; set; }

        /// <summary>Gets or sets the falcon resource name.</summary>
        [DataMember(Name = "falconResourceName", IsRequired = false)]
        public string FalconResourceName { get; set; }

        /// <summary>Gets or sets the expiration date.</summary>
        [DataMember(Name = "expirationDate", IsRequired = false)]
        public DateTime ExpirationDate { get; set; }

        /// <summary>Gets or sets the linked XRM environment information.</summary>
        [DataMember(Name = "linkedXRMEnvironmentInformation", IsRequired = false)]
        public LinkedXRMEnvironmentInformation LinkedXRMEnvironmentInformation { get; set; }
    }
}
