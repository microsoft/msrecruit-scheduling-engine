//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
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

    [ODataEntity(PluralName = "msdyn_jobopeningstageactivities", SingularName = "msdyn_jobopeningstageactivity")]
    public class JobOpeningStageActivity : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobopeningstageactivityid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobopeningstageid_value")]
        public Guid? JobOpeningStageId { get; set; }

        [DataMember(Name = "msdyn_JobopeningstageId")]
        public JobOpeningStage JobOpeningStage { get; set; }

        [DataMember(Name = "msdyn_activitytype")]
        public JobApplicationActivityType? ActivityType { get; set; }

        [DataMember(Name = "msdyn_assessmentpackageid")]
        public string AssessmentPackageId { get; set; }

        [DataMember(Name = "msdyn_assessmentpreviewurl")]
        public string AssessmentPreviewUrl { get; set; }

        [DataMember(Name = "msdyn_assessmentprovider")]
        public AssessmentProvider? AssessmentProvider { get; set; }

        [DataMember(Name = "msdyn_assessmentproviderkey")]
        public string AssessmentProviderKey { get; set; }

        [DataMember(Name = "msdyn_daystocomplete")]
        public int? DaysToComplete { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_displaylabel")]
        public string DisplayLabel { get; set; }

        [DataMember(Name = "msdyn_durationinminutes")]
        public int? DurationInMinutes { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_isenabledforcandidate")]
        public bool? IsEnabledForCandidate { get; set; }

        [DataMember(Name = "msdyn_audience")]
        public ActivityAudience? Audience { get; set; }

        [DataMember(Name = "msdyn_ordinal")]
        public int? Ordinal { get; set; }

        [DataMember(Name = "msdyn_numberofquestions")]
        public int? NumberOfQuestions { get; set; }

        [DataMember(Name = "msdyn_subordinal")]
        public int? SubOrdinal { get; set; }

        [DataMember(Name = "msdyn_required")]
        public Required? Required { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_url")]
        public string Url { get; set; }

        [DataMember(Name = "msdyn_configuration")]
        public string Configuration { get; set; }

        [DataMember(Name = "msdyn_jobopeningstageactivity_jobapplicationact")]
        public IList<JobApplicationActivity> JobApplicationActivities { get; set; }
    }
}
