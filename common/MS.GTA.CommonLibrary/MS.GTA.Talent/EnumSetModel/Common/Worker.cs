//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Worker.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.TalentEntities.Enum.Common;

    /// <summary> A contact representing a worker, such as a new hire, worker, manager, etc. </summary>
    [DataContract]
    public class Worker : DocDbEntity
    {
        [DataMember(Name = "WorkerId", EmitDefaultValue = false, IsRequired = false)]
        public string WorkerId { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public WorkerStatus? Status { get; set; }

        [DataMember(Name = "WorkerType", EmitDefaultValue = false, IsRequired = false)]
        public WorkerType? WorkerType { get; set; }

        [DataMember(Name = "SatoriId", EmitDefaultValue = false, IsRequired = false)]
        public string SatoriId { get; set; }

        [DataMember(Name = "TaxIdentificationIssuer", EmitDefaultValue = false, IsRequired = false)]
        public string TaxIdentificationIssuer { get; set; }

        [DataMember(Name = "TaxIdentificationNumber", EmitDefaultValue = false, IsRequired = false)]
        public string TaxIdentificationNumber { get; set; }

        [DataMember(Name = "LinkedInAPIURL", EmitDefaultValue = false, IsRequired = false)]
        public string LinkedInAPIURL { get; set; }

        [DataMember(Name = "HomePostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address HomePostalAddress { get; set; }

        [DataMember(Name = "ShippingPostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address ShippingPostalAddress { get; set; }

        [DataMember(Name = "BusinessPostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address BusinessPostalAddress { get; set; }
        
        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "Profession", EmitDefaultValue = false, IsRequired = false)]
        public string Profession { get; set; }

        [DataMember(Name = "Generation", EmitDefaultValue = false, IsRequired = false)]
        public string Generation { get; set; }

        [DataMember(Name = "IsPhoneContactAllowed", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsPhoneContactAllowed { get; set; }

        [DataMember(Name = "PartyType", EmitDefaultValue = false, IsRequired = false)]
        public PartyType? PartyType { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public PersonName Name { get; set; }

        [DataMember(Name = "FullName", EmitDefaultValue = false, IsRequired = false)]
        public string FullName { get; set; }

        [DataMember(Name = "EmailAlternate", EmitDefaultValue = false, IsRequired = false)]
        public string EmailAlternate { get; set; }

        [DataMember(Name = "EmailPrimary", EmitDefaultValue = false, IsRequired = false)]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "Alias", EmitDefaultValue = false, IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "PhonePrimary", EmitDefaultValue = false, IsRequired = false)]
        public string PhonePrimary { get; set; }

        [DataMember(Name = "PhoneHome", EmitDefaultValue = false, IsRequired = false)]
        public string PhoneHome { get; set; }

        [DataMember(Name = "PhoneCell", EmitDefaultValue = false, IsRequired = false)]
        public string PhoneCell { get; set; }

        [DataMember(Name = "PhoneBusiness", EmitDefaultValue = false, IsRequired = false)]
        public string PhoneBusiness { get; set; }

        [DataMember(Name = "FacebookIdentity", EmitDefaultValue = false, IsRequired = false)]
        public string FacebookIdentity { get; set; }

        [DataMember(Name = "TwitterIdentity", EmitDefaultValue = false, IsRequired = false)]
        public string TwitterIdentity { get; set; }

        [DataMember(Name = "SocialNetworkIdentity01", EmitDefaultValue = false, IsRequired = false)]
        public string SocialNetworkIdentity01 { get; set; }

        [DataMember(Name = "SocialNetworkIdentity02", EmitDefaultValue = false, IsRequired = false)]
        public string SocialNetworkIdentity02 { get; set; }

        [DataMember(Name = "WebsiteURL", EmitDefaultValue = false, IsRequired = false)]
        public string WebsiteURL { get; set; }

        [DataMember(Name = "OfficeGraphIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string OfficeGraphIdentifier { get; set; }

        [DataMember(Name = "Birthdate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? Birthdate { get; set; }

        [DataMember(Name = "Gender", EmitDefaultValue = false, IsRequired = false)]
        public Gender? Gender { get; set; }

        [DataMember(Name = "IsEmailContactAllowed", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsEmailContactAllowed { get; set; }

        [DataMember(Name = "LinkedInIdentity", EmitDefaultValue = false, IsRequired = false)]
        public string LinkedInIdentity { get; set; }

        [DataMember(Name = "ExternalReference", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalReference { get; set; }

        [DataMember(Name = "Source", EmitDefaultValue = false, IsRequired = false)]
        public Source Source { get; set; }

        //TODO
        /*[DataMember(Name = "TalentApplicationRoles", EmitDefaultValue = false, IsRequired = false)]
        public IList<TalentApplicationRole> TalentApplicationRoles { get; set; } //TODO

        [DataMember(Name = "SocialNetworkIdentities", EmitDefaultValue = false, IsRequired = false)]
        public IList<SocialNetworkIdentity> SocialNetworkIdentities { get; set; }*/
    } 
}