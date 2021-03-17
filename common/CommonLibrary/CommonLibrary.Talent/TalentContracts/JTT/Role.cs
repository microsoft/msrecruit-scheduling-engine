//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the Role Entity in JTT.
    /// </summary>
    [DataContract]
    public class Role
    {
        /// <summary>
        /// Unique Identifier of the Discipline associated to the Role.
        /// </summary>
        [DataMember(Name = "disciplineId", IsRequired = true)]
        public string DisciplineId { get; set; }

        /// <summary>
        /// Unique Identifier of the Role.
        /// </summary>
        [DataMember(Name = "roleId", IsRequired = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// Name of the Role.
        /// </summary>
        [DataMember(Name = "roleName", IsRequired = true)]
        public string RoleName { get; set; }

        /// <summary>
        /// Legacy alphanumeric identifier or code associated to Role.
        /// </summary>
        [DataMember(Name = "sapShortCode", IsRequired = true)]
        public string SAPShortCode { get; set; }

        /// <summary>
        /// Description for the Role.
        /// </summary>
        [DataMember(Name = "roleDescription")]
        public string RoleDescription { get; set; }

        /// <summary>
        /// Flag used to indicate whether the Role has been approved and released with a Job Architecture update.
        /// </summary>
        [DataMember(Name = "roleAnalysisIndicator")]
        public int RoleAnalysisIndicator { get; set; }

        /// <summary>
        /// Type used to indicate whether the Role is used for "Regular (R)" or "External Staff (E)" positions.
        /// </summary>
        [DataMember(Name = "roleType", IsRequired = true)]
        public string RoleType { get; set; }

        /// <summary>
        /// Date from which the Role is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Role is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Role - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Role is being maintained for existing and filled positions only (Yes) or can be used for new and open positions (No).
        /// </summary>
        [DataMember(Name = "exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Role is only available for usage with documented permission (Yes) or available for usage without restrictions (No).
        /// </summary>
        [DataMember(Name = "confidential")]
        public bool Confidential { get; set; }
    }
}
