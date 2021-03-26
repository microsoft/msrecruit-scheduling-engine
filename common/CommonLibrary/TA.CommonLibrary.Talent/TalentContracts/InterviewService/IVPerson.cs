//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The person data contract in IV Scheduling
    /// </summary>
    [DataContract]
    public class IVPerson
    {
        /// <summary>
        /// Gets or sets object ID.
        /// </summary>
        [DataMember(Name = "objectId", IsRequired = false, EmitDefaultValue = false)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets person given name.
        /// </summary>
        [DataMember(Name = "givenName", IsRequired = false, EmitDefaultValue = false)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets person middle name.
        /// </summary>
        [DataMember(Name = "middleName", IsRequired = false, EmitDefaultValue = false)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets person surname.
        /// </summary>
        [DataMember(Name = "surname", IsRequired = false, EmitDefaultValue = false)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets person full name.
        /// </summary>
        [DataMember(Name = "fullName", IsRequired = false, EmitDefaultValue = false)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets person email.
        /// </summary>
        [DataMember(Name = "email", IsRequired = false, EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets person alternate email.
        /// </summary>
        [DataMember(Name = "alternateEmail", IsRequired = false, EmitDefaultValue = false)]
        public string AlternateEmail { get; set; }

        /// <summary>
        /// Gets or sets person linked-in profile.
        /// </summary>
        [DataMember(Name = "linkedIn", IsRequired = false, EmitDefaultValue = false)]
        public string LinkedIn { get; set; }

        /// <summary>
        /// Gets or sets person linked-in API.
        /// </summary>
        [DataMember(Name = "linkedInAPI", IsRequired = false, EmitDefaultValue = false)]
        public string LinkedInAPI { get; set; }

        /// <summary>
        /// Gets or sets person facebook profile.
        /// </summary>
        [DataMember(Name = "facebook", IsRequired = false, EmitDefaultValue = false)]
        public string Facebook { get; set; }

        /// <summary>
        /// Gets or sets person twitter profile.
        /// </summary>
        [DataMember(Name = "twitter", IsRequired = false, EmitDefaultValue = false)]
        public string Twitter { get; set; }

        /// <summary>
        /// Gets or sets home phone.
        /// </summary>
        [DataMember(Name = "homePhone", IsRequired = false, EmitDefaultValue = false)]
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets work phone.
        /// </summary>
        [DataMember(Name = "workPhone", IsRequired = false, EmitDefaultValue = false)]
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets mobile phone.
        /// </summary>
        [DataMember(Name = "mobilePhone", IsRequired = false, EmitDefaultValue = false)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets Profession.
        /// </summary>
        [DataMember(Name = "profession", IsRequired = false, EmitDefaultValue = false)]
        public string Profession { get; set; }
    }
}
