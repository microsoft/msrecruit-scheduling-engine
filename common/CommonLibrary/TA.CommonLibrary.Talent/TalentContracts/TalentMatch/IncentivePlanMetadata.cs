//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.TalentMatch
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Incentive Plan Metadata Search Response
    /// </summary>
    [DataContract]
    public class IncentivePlanMetadata
    {
        /// <summary>
        /// Gets or sets Organization
        /// </summary>
        [DataMember(Name = "organization")]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets Profession
        /// </summary>
        [DataMember(Name = "profession")]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets Discipline
        /// </summary>
        [DataMember(Name = "discipline")]
        public string Discipline { get; set; }

        /// <summary>
        /// Gets or sets StandardTitle
        /// </summary>
        [DataMember(Name = "standardTitle")]
        public string StandardTitle { get; set; }

        /// <summary>
        /// Gets or sets IncentivePlan
        /// </summary>
        [DataMember(Name = "incentivePlan")]
        public string IncentivePlan { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        [DataMember(Name = "level")]
        public string Level { get; set; }
    }
}
