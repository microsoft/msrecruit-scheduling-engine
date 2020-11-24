﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [ODataEntity(PluralName = "msdyn_jobapplicationoffernotes", SingularName = "msdyn_jobapplicationoffernote")]
    public class JobApplicationOfferNote : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationoffernoteid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationid")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_workerid")]
        public Worker Worker { get; set; }

        [DataMember(Name = "msdyn_offernote")]
        public string Note { get; set; }

        [DataMember(Name = "msdyn_jobapplicationoffernote_Annotations")]
        public IList<Annotation> Annotation { get; set; }
    }
}
