//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Application Stage data contract.
    /// </summary>
    [DataContract]
    public class ApplicationStage : TalentBaseContract
    {       
        /// <summary>
        /// Gets or sets job stage.
        /// </summary>
        [DataMember(Name = "stage")]
        public JobStage Stage { get; set; }

        /// <summary>
        /// Gets or sets order.
        /// </summary>
        [DataMember(Name = "order")]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [DataMember(Name = "displayName", IsRequired = false, EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the stage activities.
        /// </summary>
        [DataMember(Name = "stageActivities", IsRequired = false, EmitDefaultValue = false)]
        public IList<StageActivity> StageActivities { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates that the stage is acitve stage or not. 
        /// </summary>
        [DataMember(Name = "isActiveStage", IsRequired = false, EmitDefaultValue = false)]
        public bool IsActiveStage { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [DataMember(Name = "totalActivities", IsRequired = false, EmitDefaultValue = false)]
        public int TotalActivities { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [DataMember(Name = "completedActivities", IsRequired = false, EmitDefaultValue = false)]
        public int CompletedActivities { get; set; }

        /// <summary>
        /// Gets or sets Status Changed Date Time.
        /// </summary>
        [DataMember(Name = "lastCompletedActivityDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? LastCompletedActivityDateTime { get; set; }
    }
}
