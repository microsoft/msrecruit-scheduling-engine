//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [ODataNamespace(Namespace = "Microsoft.Dynamics.CRM")]
    [ODataEntity(PluralName = "systemusers", SingularName = "systemuser")]
    public class SystemUser : ODataEntity
    {
        [Key]
        [DataMember(Name = "systemuserid")]
        public Guid? SystemUserId { get { return RecId; } set { RecId = value; } }

        [DataMember(Name = "ownerid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "azureactivedirectoryobjectid")]
        public Guid? AzureActiveDirectoryObjectId { get; set; }

        [DataMember(Name = "applicationid")]
        public Guid? ApplicationId { get; set; }

        [DataMember(Name = "userpuid")]
        public string Puid { get; set; }

        [DataMember(Name = "issyncwithdirectory")]
        public bool? IsSyncWithDirectory { get; set; }

        [DataMember(Name = "isdisabled")]
        public bool? IsDisabled { get; set; }

        [DataMember(Name = "islicensed")]
        public bool? IsLicensed { get; set; }

        [DataMember(Name = "setupuser")]
        public bool? IsSetupUser { get; set; }

        [DataMember(Name = "accessmode")]
        public SystemUserAccessMode? AccessMode { get; set; }

        [DataMember(Name = "userlicensetype")]
        public int? UserLicenseType { get; set; }

        [DataMember(Name = "caltype")]
        public SystemUserLicenseType? LicenseType { get; set; }

        [DataMember(Name = "disabledreason")]
        public string DisabledReason { get; set; }

        [DataMember(Name = "internalemailaddress")]
        public string PrimaryEmail { get; set; }

        [DataMember(Name = "windowsliveid")]
        public string WindowsLiveId { get; set; }

        [DataMember(Name = "domainname")]
        public string DomainName { get; set; }

        [DataMember(Name = "fullname")]
        public string FullName { get; set; }

        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }

        [DataMember(Name = "middlename")]
        public string MiddleName { get; set; }

        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "yomifullname")]
        public string YomiFullName { get; set; }

        [DataMember(Name = "yomifirstname")]
        public string YomiFirstName { get; set; }

        [DataMember(Name = "yomimiddlename")]
        public string YomiMiddleName { get; set; }

        [DataMember(Name = "yomilastname")]
        public string YomiLastName { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "homephone")]
        public string HomePhone { get; set; }

        [DataMember(Name = "mobilephone")]
        public string MobilePhone { get; set; }

        [DataMember(Name = "address1_telephone1")]
        public string Address1Telephone1 { get; set; }

        [DataMember(Name = "address1_telephone2")]
        public string Address1Telephone2 { get; set; }

        [DataMember(Name = "address1_telephone3")]
        public string Address1Telephone3 { get; set; }

        [DataMember(Name = "address2_telephone1")]
        public string Address2Telephone1 { get; set; }

        [DataMember(Name = "address2_telephone2")]
        public string Address2Telephone2 { get; set; }

        [DataMember(Name = "address2_telephone3")]
        public string Address2Telephone3 { get; set; }

        [DataMember(Name = "_businessunitid_value")]
        public Guid? BusinessUnitId { get; set; }

        //// [DataMember(Name = "businessunitid")]
        //// public BusinessUnit BusinessUnit { get; set; }

        [DataMember(Name = "_parentsystemuserid_value")]
        public Guid? ParentSystemUserId { get; set; }

        [DataMember(Name = "parentsystemuserid")]
        public SystemUser ParentSystemUser { get; set; }

        [DataMember(Name = "user_parent_user")]
        public IList<SystemUser> ChildrenSystemUsers { get; set; }

        [DataMember(Name = "systemuserroles_association")]
        public IList<Role> Roles { get; set; }
    }
}
