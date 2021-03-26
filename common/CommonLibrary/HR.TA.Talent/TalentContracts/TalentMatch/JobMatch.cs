//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using HR.TA.Common.TalentEntities.Common;
using Newtonsoft.Json;

namespace HR.TA.Talent.TalentContracts.TalentMatch
{
    /// <summary>
    /// Job Match
    /// </summary>
    [DataContract]
    public class JobMatch
    {
        /// <summary>Gets or sets Job opening id.</summary>
        [DataMember(Name = "jobOpeningId", EmitDefaultValue = false, IsRequired = true)]
        public string JobOpeningId { get; set; }

        /// <summary>Gets or sets external job opening id.</summary>
        [DataMember(Name = "externalJobOpeningId", EmitDefaultValue = false, IsRequired = true)]
        public string ExternalJobOpeningId { get; set; }

        /// <summary>Gets or sets job opening description.</summary>
        [DataMember(Name = "description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        /// <summary>Gets or sets job opening Location.</summary>
        [DataMember(Name = "jobLocation", EmitDefaultValue = false, IsRequired = false)]
        public Address JobLocation { get; set; }

        /// <summary>Gets or sets job opening Title.</summary>
        [DataMember(Name = "jobTitle", EmitDefaultValue = false, IsRequired = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets JobOpeningProperties
        /// </summary>
        [DataMember(Name = "jobOpeningProperties", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningProperties JobOpeningProperties { get; set; }

        /// <summary>
        /// Gets or sets Job Opening
        /// </summary>
        [DataMember(Name = "computeResult", EmitDefaultValue = false, IsRequired = false)]
        public List<JobSkill> ComputeResult { get; set; }

        /// <summary>
        /// Gets or sets score
        /// </summary>
        [DataMember(Name = "score", EmitDefaultValue = false, IsRequired = true)]
        public double Score { get; set; }
    }
}
