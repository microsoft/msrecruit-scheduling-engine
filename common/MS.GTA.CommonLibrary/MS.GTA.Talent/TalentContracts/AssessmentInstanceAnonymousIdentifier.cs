// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AssessmentInstanceAnonymousIdentifier.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it.
namespace MS.GTA.Common.Attract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Assessment instance anonymous identifier contract.</summary>
    [DataContract]
    public class AssessmentInstanceAnonymousIdentifier
    {
        /// <summary>
        /// Gets or sets the anonymous identifier ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets external assessment engine provider id.
        /// </summary>
        [DataMember(Name = "provider", IsRequired = true)]
        public int Provider { get; set; }

        /// <summary>
        /// Gets or sets the mapped AssessmentReport entity ID.
        /// </summary>
        [DataMember(Name = "assessmentReportId", IsRequired = true)]
        public string AssessmentReportId { get; set; }

        /// <summary>
        /// Gets or sets the mapped EnvironmentId.
        /// </summary>
        [DataMember(Name = "environmentId")]
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the mapped EnvironmentId.
        /// </summary>
        [DataMember(Name = "tenantObjectId")]
        public string TenantObjectId { get; set; }
    }
}
