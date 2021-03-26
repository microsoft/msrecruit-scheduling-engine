//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.Contracts;

    /// <summary>
    /// The job data contract.
    /// </summary>
    [DataContract]
    public class JobTemplate : TalentBaseContract
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets name.</summary>
        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }

        /// <summary>Gets or sets displayName.</summary>
        [DataMember(Name = "displayName", IsRequired = false)]
        public string DisplayName { get; set; }

        /// <summary>Gets or sets valid from.</summary>
        [DataMember(Name = "validFrom", IsRequired = false)]
        public DateTime ValidFrom { get; set; }

        /// <summary>Gets or sets valid to.</summary>
        [DataMember(Name = "validTo", IsRequired = false)]
        public DateTime ValidTo { get; set; }

        /// <summary>Gets or sets a value indicating whether job template is active.</summary>
        [DataMember(Name = "isActive", IsRequired = false)]
        public bool IsActive { get; set; }

        /// <summary>Gets or sets a value indicating whether job template is default.</summary>
        [DataMember(Name = "isDefault", IsRequired = false)]
        public bool IsDefault { get; set; }

        /// <summary>Gets or sets template reference. </summary>
        [DataMember(Name = "templateReference", IsRequired = false)]
        public string TemplateReference { get; set; }

        /// <summary>Gets or sets stageTemplates. </summary>
        [DataMember(Name = "stageTemplates", IsRequired = false)]
        public IList<JobStageTemplate> StageTemplates { get; set; }
    }
}
