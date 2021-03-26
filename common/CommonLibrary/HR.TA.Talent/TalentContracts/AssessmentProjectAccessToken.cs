//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it.
namespace HR.TA..Common.Attract.Contract
{
    using System.Runtime.Serialization;

    /// <summary> Assessment provider access token</summary>
    [DataContract]
    public class AssessmentProjectAccessToken
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the AAD user who owns this OAUTH connection.
        /// </summary>
        [DataMember(Name = "user", IsRequired = true)]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Tenant.
        /// </summary>
        [DataMember(Name = "tenant", IsRequired = false)]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets external assessment engine provider id.
        /// </summary>
        [DataMember(Name = "provider", IsRequired = true)]
        public int Provider { get; set; }

        /// <summary>
        /// Gets or sets assessment access token.
        /// </summary>
        [DataMember(Name = "projectAccessToken", IsRequired = true)]
        public string ProjectAccessToken { get; set; }

        /// <summary>
        /// Gets or sets the project Id.
        /// </summary>
        [DataMember(Name = "projectID", IsRequired = true)]
        public string ProjectID { get; set; }
    }
}
