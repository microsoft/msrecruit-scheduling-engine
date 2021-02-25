//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.TalentEntities.Enum.Common;

    /// <summary>
    /// Team member 
    /// </summary>
    [DataContract]
    public class TeamMember : AADUser
    {
        /// <summary>
        /// Gets or sets Role of team member
        /// </summary>
        [DataMember(Name = "Role")]
        public JobParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets User action.
        /// </summary>
        [DataMember(Name = "UserAction")]
        public UserAction UserAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is member of activity.
        /// </summary>
        [DataMember(Name = "IsMemberOfActivity")]
        public bool IsMemberOfActivity { get; set; }
    }

    /// <summary>
    /// AAD User to map response form graph. Name starts with lower case to support user directory response.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class AADUser : TalentBaseContract
    {
        /// <summary>Gets or sets the name.</summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets the title.</summary>
        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>Gets or sets the email.</summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>Gets or sets the given name.</summary>
        [DataMember(Name = "given", EmitDefaultValue = false)]
        public string GivenName { get; set; }

        /// <summary>Gets or sets the mobile phone.</summary>
        [DataMember(Name = "mobilePhone", EmitDefaultValue = false)]
        public string MobilePhone { get; set; }

        /// <summary>Gets or sets the office location.</summary>
        [DataMember(Name = "officeLocation")]
        public string OfficeLocation { get; set; }

        /// <summary>Gets or sets the user principal name.</summary>
        [DataMember(Name = "userPrincipalName")]
        public string UserPrincipalName { get; set; }

        /// <summary>Gets or sets the mail nickname.</summary>
        [DataMember(Name = "mailNickname", IsRequired = false, EmitDefaultValue = false)]
        public string MailNickname { get; set; }

        /// <summary>Gets or sets the external worker id.</summary>
        [DataMember(Name = "externalWorkerId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalWorkerId { get; set; }

        /// <summary>Gets or sets the external worker source.</summary>
        [DataMember(Name = "externalWorkerSource", IsRequired = false, EmitDefaultValue = false)]
        public Source? ExternalWorkerSource { get; set; }
    }
}
