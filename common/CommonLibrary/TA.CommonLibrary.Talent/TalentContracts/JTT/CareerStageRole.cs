//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the CareerStage to Role Mapping Entity in JTT.
    /// </summary>
    [DataContract]
    public class CareerStageRole
    {
        /// <summary>
        /// Unique Identifier of the Career Stage.
        /// </summary>
        [DataMember(Name = "careerStageId", IsRequired = true)]
        public string CareerStageId { get; set; }

        /// <summary>
        /// Unique Identifier of the Role.
        /// </summary>
        [DataMember(Name = "roleId", IsRequired = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// Date from which the Career Stage to Role Mapping is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Career Stage to Role Mapping is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Career Stage - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }
    }
}
