//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentEntities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.DocumentDB.Contracts;
    using HR.TA..Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA..Common.TalentEntities.Enum.Common;
    using HR.TA..Common.Web.Contracts;

    /// <summary> A contact representing a worker, such as a new hire, worker, manager, etc. </summary>
    [DataContract]
    public class Worker : DocDbEntity
    {
        [DataMember(Name = "WorkerId", EmitDefaultValue = false, IsRequired = false)]
        public string WorkerId { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public WorkerStatus? Status { get; set; }

        [DataMember(Name = "Profession", EmitDefaultValue = false, IsRequired = false)]
        public string Profession { get; set; }

        [DataMember(Name = "IsPhoneContactAllowed", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsPhoneContactAllowed { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public PersonName Name { get; set; }

        [DataMember(Name = "FullName", EmitDefaultValue = false, IsRequired = false)]
        public string FullName { get; set; }

        [DataMember(Name = "EmailPrimary", EmitDefaultValue = false, IsRequired = false)]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "Alias", EmitDefaultValue = false, IsRequired = false)]
        public string Alias { get; set; }

        [DataMember(Name = "PhonePrimary", EmitDefaultValue = false, IsRequired = false)]
        public string PhonePrimary { get; set; }

        [DataMember(Name = "OfficeGraphIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string OfficeGraphIdentifier { get; set; }

        [DataMember(Name = "TeamsIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string TeamsIdentifier { get; set; }

        [DataMember(Name = "IsEmailContactAllowed", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsEmailContactAllowed { get; set; }
    }
}