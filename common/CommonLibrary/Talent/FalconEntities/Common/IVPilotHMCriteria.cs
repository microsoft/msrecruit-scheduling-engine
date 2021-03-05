//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Common.DocumentDB.Contracts;
using System.Runtime.Serialization;

namespace Talent.FalconEntities.Common
{
    /// <summary>
    /// IV Pilot Hiring Manger criteria for ring 0,1,2..
    /// </summary>
    [DataContract]
    public class IVPilotHMCriteria : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the Hiring Manager name for pilot 
        /// </summary>
        [DataMember(Name = "HiringManagerName", EmitDefaultValue = false, IsRequired = false)]
        public string HiringManagerName { get; set; }

        /// <summary>
        /// Gets or sets the Hiring Manager email for pilot 
        /// </summary>
        [DataMember(Name = "HiringManagerEmail", EmitDefaultValue = false, IsRequired = false)]
        public string HiringManagerEmail { get; set; }

        /// <summary>
        /// Gets or sets the requisiton type for pilot 
        /// </summary>
        [DataMember(Name = "RequisitionType", EmitDefaultValue = false, IsRequired = false)]
        public string RequisitionType { get; set; }

        /// <summary>
        /// Gets or sets the icims person id for the Hiring Manager for pilot 
        /// </summary>
        [DataMember(Name = "HiringManagerExternalId", EmitDefaultValue = false, IsRequired = false)]
        public string HiringManagerExternalId { get; set; }

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
