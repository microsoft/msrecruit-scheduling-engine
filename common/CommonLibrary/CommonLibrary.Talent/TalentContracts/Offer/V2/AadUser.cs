//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// AAD User to map response form graph. Name starts with lower case to support user directory response.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class AadUser
    {
        /// <summary>Gets or sets the name.</summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets the title.</summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>Gets or sets the email.</summary>
        [DataMember(Name = "email", IsRequired = false, EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>Gets or sets the given name.</summary>
        [DataMember(Name = "given", IsRequired = false, EmitDefaultValue = false)]
        public string GivenName { get; set; }

        /// <summary>Gets or sets the full name.</summary>
        [DataMember(Name = "fullName", IsRequired = false, EmitDefaultValue = false)]
        public string FullName { get; set; }

        /// <summary>Gets or sets the surname.</summary>
        [DataMember(Name = "surname", IsRequired = false, EmitDefaultValue = false)]
        public string Surname { get; set; }

        /// <summary>Gets or sets the middle name.</summary>
        [DataMember(Name = "middleName", IsRequired = false, EmitDefaultValue = false)]
        public string MiddleName { get; set; }

        /// <summary>Gets or sets the mobile phone.</summary>
        [DataMember(Name = "mobilePhone", IsRequired = false, EmitDefaultValue = false)]
        public string MobilePhone { get; set; }

        /// <summary>Gets or sets the office location.</summary>
        [DataMember(Name = "officeLocation", IsRequired = false)]
        public string OfficeLocation { get; set; }

        /// <summary>Gets or sets the user principal name.</summary>
        [DataMember(Name = "userPrincipalName", IsRequired = false)]
        public string UserPrincipalName { get; set; }
    }
}
