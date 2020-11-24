// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Role.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp.Model.Metadata;

    [ODataNamespace(Namespace = "Microsoft.Dynamics.CRM")]
    [ODataEntity(PluralName = "roles", SingularName = "role")]
    public class Role : ODataEntity
    {
        [Key]
        [DataMember(Name = "roleid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "roleidunique")]
        public Guid? RoleIdUnique { get; set; }

        [DataMember(Name = "organizationid")]
        public Guid? OrganizationId { get; set; }

        //// [DataMember(Name = "organizationid_organization")]
        //// public Organization Organization { get; set; }

        [DataMember(Name = "solutionid")]
        public Guid? SolutionId { get; set; }

        //// [DataMember(Name = "solution_role")]
        //// public Solution Solution { get; set; }

        [DataMember(Name = "canbedeleted")]
        public BooleanManagedProperty CanBeDeleted { get; set; }

        [DataMember(Name = "iscustomizable")]
        public BooleanManagedProperty IsCustomizable { get; set; }

        [DataMember(Name = "ismanaged")]
        public bool? IsManaged { get; set; }

        [DataMember(Name = "componentstate")]
        public int? ComponentState { get; set; }

        [DataMember(Name = "_businessunitid_value")]
        public Guid? BusinessUnitId { get; set; }

        //// [DataMember(Name = "businessunitid")]
        //// public BusinessUnit BusinessUnit { get; set; }

        [DataMember(Name = "_parentroleid_value")]
        public Guid? ParentRoleId { get; set; }

        [DataMember(Name = "parentroleid")]
        public Role ParentRole { get; set; }

        [DataMember(Name = "role_parent_role")]
        public IList<Role> ChildrenRoles { get; set; }

        [DataMember(Name = "_parentrootroleid_value")]
        public Guid? ParentRootRoleId { get; set; }

        [DataMember(Name = "parentrootroleid")]
        public Role ParentRootRole { get; set; }

        [DataMember(Name = "role_parent_root_role")]
        public IList<Role> RootChildrenRoles { get; set; }

        [DataMember(Name = "_roletemplateid_value")]
        public Guid? RoleTemplateId { get; set; }

        //// [DataMember(Name = "roletemplateid")]
        //// public RoleTemplate RoleTemplateId { get; set; }

        [DataMember(Name = "systemuserroles_association")]
        public IList<SystemUser> SystemUsers { get; set; }

        //// [DataMember(Name = "roleprivileges_association")]
        //// public IList<Privilege> Privileges { get; set; }

        //// [DataMember(Name = "appmoduleroles_association")]
        //// public IList<AppModule> AppModules { get; set; }

        //// [DataMember(Name = "teamroles_association")]
        //// public IList<Team> Teams { get; set; }
    }
}
