//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Common
{
    /// <summary>
    /// IV Pilot criteria for ring 0,1,2..
    /// </summary>
    [DataContract]
    public class IVPilotCriteria : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the recruiter name for pilot 
        /// </summary>
        [DataMember(Name = "RecruiterName", EmitDefaultValue = false, IsRequired = false)]
        public string RecruiterName { get; set; }
        
        /// <summary>
        /// Gets or sets the recruiter email for pilot 
        /// </summary>
        [DataMember(Name = "RecruiterEmail", EmitDefaultValue = false, IsRequired = false)]
        public string RecruiterEmail { get; set; }

        /// <summary>
        /// Gets or sets the requisiton type for pilot 
        /// </summary>
        [DataMember(Name = "RequisitionType", EmitDefaultValue = false, IsRequired = false)]
        public string RequisitionType { get; set; }

        /// <summary>
        /// Gets or sets the icims person id for the recruiter for pilot 
        /// </summary>
        [DataMember(Name = "RecruiterExternalId", EmitDefaultValue = false, IsRequired = false)]
        public string RecruiterExternalId { get; set; }

        /// <summary>
        /// Gets or sets the flag to check if the record has been processed 
        /// </summary>
        [DataMember(Name = "IsProcessed", EmitDefaultValue = false, IsRequired = false)]
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the pilot type for the requisition 
        /// </summary>
        [DataMember(Name = "PilotType", EmitDefaultValue = false, IsRequired = false)]
        public IVRequisitionPilotType PilotType { get; set; }

    }
}
