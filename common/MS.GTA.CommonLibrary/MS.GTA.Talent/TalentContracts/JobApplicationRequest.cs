//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationRequest.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Job Application Request
    /// </summary>
    [DataContract]
    public class JobApplicationRequest
    {
        /// <summary>
        /// Gets or sets the collection of statuses
        /// </summary>
        [DataMember(Name = "applicationStatuses", IsRequired = false, EmitDefaultValue = false)]
        public IList<JobApplicationStatus> ApplicationStatuses { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "searchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

        /// <summary>
        ///  Gets or sets stage order
        /// </summary>
        [DataMember(Name = "stageOrder", IsRequired = false, EmitDefaultValue = false)]
        public int StageOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only prospect job applications are returned. 
        /// </summary>
        [DataMember(Name = "prospectOnly", IsRequired = false, EmitDefaultValue = false)]
        public bool ProspectOnly { get; set; }

    }
}
