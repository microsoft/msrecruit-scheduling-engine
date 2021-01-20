//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AssessmentConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Assessment activity.
    /// </summary>
    [DataContract]
    public class AssessmentConfiguration
    {
        /// <summary>
        /// Gets or sets a list of job opening assessments.
        /// </summary>
        [DataMember(Name = "jobAssessments", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<JobAssessment> JobAssessments { get; set; }
    }
}
