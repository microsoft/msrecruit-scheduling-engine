//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GraphPerson.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// GraphPerson properties
    /// </summary>
    [DataContract]
    public class GraphPerson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphPerson" /> class.
        /// </summary>
        public GraphPerson()
        {
        }

        /// <summary>
        /// Gets or sets the person's display name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the person's id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the person's jobTitle.
        /// </summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the person's mail.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the person's givenName.
        /// </summary>
        [DataMember(Name = "givenName", IsRequired = false, EmitDefaultValue = false)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the person's mobilePhone.
        /// </summary>
        [DataMember(Name = "mobilePhone", IsRequired = false, EmitDefaultValue = false)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the person's officeLocation.
        /// </summary>
        [DataMember(Name = "officeLocation", IsRequired = false, EmitDefaultValue = false)]
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Gets or sets the person's preferredLanguage.
        /// </summary>
        [DataMember(Name = "preferredLanguage", IsRequired = false, EmitDefaultValue = false)]
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// Gets or sets the person's surname.
        /// </summary>
        [DataMember(Name = "surname", IsRequired = false, EmitDefaultValue = false)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the person's userPrincipalName.
        /// </summary>
        [DataMember(Name = "userPrincipalName", IsRequired = false, EmitDefaultValue = false)]
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the person's surname.
        /// </summary>
        [DataMember(Name = "InvitationResponseStatus", IsRequired = false, EmitDefaultValue = false)]
        public InvitationResponseStatus InvitationResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the person's userPrincipalName.
        /// </summary>
        [DataMember(Name = "Comments", IsRequired = false, EmitDefaultValue = false)]
        public string Comments { get; set; }
    }
}
