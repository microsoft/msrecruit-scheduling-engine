//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CandidateWorkExperience
    {
        [DataMember(Name = "CandidateWorkExperienceID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateWorkExperienceID { get; set; }

        [DataMember(Name = "Title", EmitDefaultValue = false, IsRequired = false)]
        public string Title { get; set; }

        [DataMember(Name = "Organization", EmitDefaultValue = false, IsRequired = false)]
        public string Organization { get; set; }

        [DataMember(Name = "Location", EmitDefaultValue = false, IsRequired = false)]
        public string Location { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "IsCurrentPosition", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsCurrentPosition { get; set; }

        [DataMember(Name = "FromMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "ToMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ToMonthYear { get; set; }
    }
}
