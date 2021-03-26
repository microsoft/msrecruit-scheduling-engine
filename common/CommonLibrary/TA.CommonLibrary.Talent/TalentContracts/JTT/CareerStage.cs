//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the CareerStage Entity in JTT.
    /// </summary>
    [DataContract]
    public class CareerStage
    {
        /// <summary>
        /// Unique Identifier of the Career Stage.
        /// </summary>
        [DataMember(Name = "careerStageId", IsRequired = true)]
        public string CareerStageId { get; set; }

        /// <summary>
        /// Name of the Career Stage.
        /// </summary>
        [DataMember(Name = "careerStageName", IsRequired = true)]
        public string CareerStageName { get; set; }

        /// <summary>
        /// Prefix portion of the Career Stage.
        /// </summary>
        [DataMember(Name = "careerStagePrefix", IsRequired = true)]
        public string CareerStagePrefix { get; set; }

        /// <summary>
        /// Suffix portion of the Career Stage.
        /// </summary>
        [DataMember(Name = "careerStageSuffix", IsRequired = true)]
        public string CareerStageSuffix { get; set; }

        /// <summary>
        /// Unique identifier to indicate if the Career Stage is IC (0), Manager(1) or Manager of Manager(2).
        /// </summary>
        [DataMember(Name = "managerIndicatorId")]
        public string ManagerIndicatorId { get; set; }

        /// <summary>
        /// Name associated to the <see cref="ManagerIndicatorId"/>.
        /// </summary>
        [DataMember(Name = "managerIndicator", IsRequired = true)]
        public string ManagerIndicator { get; set; }

        /// <summary>
        /// Date from which the Career Stage is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Career Stage is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Career Stage - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Career Stage is being maintained for existing and filled positions only (Yes) or can be used for new and open positions (No).
        /// </summary>
        [DataMember(Name = "exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Career Stage is only available for usage with documented permission (Yes) or available for usage without restrictions (No).
        /// </summary>
        [DataMember(Name = "confidential")]
        public bool Confidential { get; set; }
    }
}
