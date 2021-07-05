//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using HR.TA.Common.Contracts;
    using HR.TA.Common.TalentEntities.Common;

    /// <summary>
    /// The person data contract.
    /// </summary>
    [DataContract]
    public class Person : TalentBaseContract
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

        /// <summary>
        /// Gets or sets the mail nickname.
        /// </summary>
        [DataMember(Name = "mailNickname", IsRequired = false, EmitDefaultValue = false)]
        public string MailNickname { get; set; }

        /// <summary>
        /// Gets or sets the external worker id.
        /// </summary>
        [DataMember(Name = "externalWorkerId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalWorkerId { get; set; }

        /// <summary>
        /// Gets or sets Mailing Postal Address.
        /// </summary>
        [DataMember(Name = "mailingPostalAddress", IsRequired = false, EmitDefaultValue = false)]
        public Address MailingPostalAddress { get; set; }

        /// <summary>
        /// Gets or sets Other Postal Address.
        /// </summary>
        [DataMember(Name = "otherPostalAddress", IsRequired = false, EmitDefaultValue = false)]
        public Address OtherPostalAddress { get; set; }
    }
}
