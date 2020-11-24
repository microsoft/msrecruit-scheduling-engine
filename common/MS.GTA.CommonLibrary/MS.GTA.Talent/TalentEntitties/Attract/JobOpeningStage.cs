//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobopeningstages", SingularName = "msdyn_jobopeningstage")]
    public class JobOpeningStage : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobopeningstageid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobopeningid_value")]
        public Guid? JobOpeningId { get; set; }

        [DataMember(Name = "msdyn_JobopeningId")]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_displayname")]
        public string DisplayName { get; set; }

        [DataMember(Name = "msdyn_ordinal")]
        public int? Ordinal { get; set; }

        [DataMember(Name = "msdyn_plugindata")]
        public string PluginData { get; set; }

        [DataMember(Name = "msdyn_jobopeningstage_jobopeningstage")]
        public IList<JobOpeningStageActivity> JobOpeningStageActivities { get; set; }

        [DataMember(Name = "msdyn_jobopeningstage_jobapplication")]
        public IList<JobApplication> JobApplicationsInStage { get; set; }
    }
}
