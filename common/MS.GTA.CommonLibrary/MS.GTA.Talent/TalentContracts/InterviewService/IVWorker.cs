//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace MS.GTA.Talent.TalentContracts.InterviewService
{
    using MS.GTA.Common.TalentEntities.Common;
    using System.Runtime.Serialization;

    /// <summary>
    /// IV Worker
    /// </summary>
    [DataContract]
    public class IVWorker
    {
        [DataMember(Name = "workerId", EmitDefaultValue = false, IsRequired = false)]
        public string WorkerId { get; set; }

        [DataMember(Name = "profession", EmitDefaultValue = false, IsRequired = false)]
        public string Profession { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = false)]
        public PersonName Name { get; set; }

        [DataMember(Name = "fullName", EmitDefaultValue = false, IsRequired = false)]
        public string FullName { get; set; }

        [DataMember(Name = "emailPrimary", EmitDefaultValue = false, IsRequired = false)]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "alias", EmitDefaultValue = false, IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "phonePrimary", EmitDefaultValue = false, IsRequired = false)]
        public string PhonePrimary { get; set; }

        [DataMember(Name = "officeGraphIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string OfficeGraphIdentifier { get; set; }

        [DataMember(Name = "isEmailContactAllowed", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsEmailContactAllowed { get; set; }
    }
}
