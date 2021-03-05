//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it.
namespace Common.Attract.Contract
{
    using System.Runtime.Serialization;

    /// <summary> Assessment provider connection </summary>
    [DataContract]
    public class AssessmentProviderConnection
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
        /// Gets or sets the AAD Tenant id.
        /// </summary>
        [DataMember(Name = "tenant", IsRequired = false)]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets external assessment engine provider id.
        /// </summary>
        [DataMember(Name = "provider", IsRequired = true)]
        public int Provider { get; set; }

        /// <summary>
        /// Gets or sets assessment provider OAUTH authentication token.
        /// </summary>
        [DataMember(Name = "authToken", IsRequired = true)]
        public string AuthToken { get; set; } 
    }
}
