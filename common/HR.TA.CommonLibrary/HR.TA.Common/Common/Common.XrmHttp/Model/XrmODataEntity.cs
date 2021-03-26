//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp.Model;

    [DataContract]
    [ODataNamespace(Namespace = "Microsoft.Dynamics.CRM")]
    public class XrmODataEntity : ODataEntity
    {
        [DataMember(Name = "_createdby_value")]
        public Guid? XrmCreatedById { get; set; }

        [DataMember(Name = "createdby")]
        public SystemUser XrmCreatedBy { get; set; }

        [DataMember(Name = "_createdonbehalfby_value")]
        public Guid? XrmCreatedOnBehalfById { get; set; }

        [DataMember(Name = "createdonbehalfby")]
        public SystemUser XrmCreatedOnBehalfBy { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime? XrmCreatedOn { get; set; }
        
        [DataMember(Name = "overriddencreatedon")]
        public DateTime? XrmOverriddenCreatedOn { get; set; }
        
        [DataMember(Name = "_modifiedby_value")]
        public Guid? XrmModifiedById { get; set; }

        [DataMember(Name = "modifiedby")]
        public SystemUser XrmModifiedBy { get; set; }

        [DataMember(Name = "_modifiedonbehalfby_value")]
        public object XrmModifiedOnBehalfById { get; set; }

        [DataMember(Name = "modifiedonbehalfby")]
        public SystemUser XrmModifiedOnBehalfBy { get; set; }

        [DataMember(Name = "modifiedon")]
        public DateTime? XrmModifiedOn { get; set; }
        
        [DataMember(Name = "_ownerid_value")]
        public Guid? XrmOwnerId { get; set; }

        [DataMember(Name = "ownerid")]
        public SystemUser XrmOwner { get; set; }

        [DataMember(Name = "_owningbusinessunit_value")]
        public Guid? XrmOwningBusinessUnitId { get; set; }
        
        [DataMember(Name = "_owningteam_value")]
        public Guid? XrmOwningTeamId { get; set; }
        
        [DataMember(Name = "_owninguser_value")]
        public Guid? XrmOwningUserId { get; set; }

        [DataMember(Name = "owninguser")]
        public SystemUser XrmOwningUser { get; set; }

        [DataMember(Name = "importsequencenumber")]
        public object XrmImportSequenceNumber { get; set; }
        
        [DataMember(Name = "statecode")]
        public int XrmStateCode { get; set; }
        
        [DataMember(Name = "statuscode")]
        public int XrmStatusCode { get; set; }
        
        [DataMember(Name = "timezoneruleversionnumber")]
        public object XrmTimeZoneRuleVersionNumber { get; set; }
        
        [DataMember(Name = "utcconversiontimezonecode")]
        public object XrmUtcConversionTimeZone { get; set; }
        
        [DataMember(Name = "versionnumber")]
        public int XrmVersionNumber { get; set; }
    }
}
