//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Profession.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the Profession Entity in JTT.
    /// </summary>
    [DataContract]
    public class Profession
    {
        /// <summary>
        /// Unique Identifier for the Profession.
        /// </summary>
        [DataMember(Name = "professionId", IsRequired = true)]
        public string ProfessionId { get; set; }

        /// <summary>
        /// Name of the Profession.
        /// </summary>
        [DataMember(Name = "professionName", IsRequired = true)]
        public string ProfessionName { get; set; }

        /// <summary>
        /// Description of the Profession.
        /// </summary>
        [DataMember(Name = "professionDescription")]
        public string ProfessionDescription { get; set; }

        /// <summary>
        /// Date from which the Profession is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Profession is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Profession - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Profession is being maintained for existing and filled positions only (Yes) or can be used for new and open positions (No).
        /// </summary>
        [DataMember(Name = "exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Profession is only available for usage with documented permission (Yes) or available for usage without restrictions (No).
        /// </summary>
        [DataMember(Name = "confidential")]
        public bool Confidential { get; set; }
    }
}
