// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationInterviewMetadata.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it.
namespace MS.GTA.Common.Attract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job application interview metadata.
    /// </summary>
    [DataContract]
    public class JobApplicationInterviewMetadata
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>Gets or sets the application id.</summary>
        [DataMember(Name = nameof(JobApplicationID))]
        public string JobApplicationID { get; set; }

        /// <summary>Gets or sets the interview location.</summary>
        [DataMember(Name = nameof(Location))]
        public string Location { get; set; }

        /// <summary>Gets or sets the comment data.</summary>
        [DataMember(Name = nameof(Comment))]
        public string Comment { get; set; }

        /// <summary> Gets or sets dates. </summary>
        [DataMember(Name = nameof(ScheduleDates), IsRequired = false, EmitDefaultValue = false)]
        public string[] ScheduleDates { get; set; }

        /// <summary> Gets or sets time zone name. </summary>
        [DataMember(Name = nameof(TimezoneName), IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneName { get; set; }
    }
}
