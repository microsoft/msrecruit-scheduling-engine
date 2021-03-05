//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Common.DocumentDB.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Talent.FalconEntities.Common
{
    /// <summary> A contact representing a pilot requisition. </summary>
    [DataContract]
    public class IVPilotRequisition : DocDbEntity
    {   
        /// <summary>
        /// Gets or sets the external id for requisition 
        /// </summary>
        [DataMember(Name = "RequisitionId", EmitDefaultValue = false, IsRequired = false)]
        public string RequisitionId  { get; set; }

        /// <summary>
        /// Gets or sets the pilot type
        /// </summary>
        [DataMember(Name = "PilotType", EmitDefaultValue = false, IsRequired = false)]
        public IVRequisitionPilotType? PilotType { get; set; }

        /// <summary>
        /// Gets or sets the comments for requisition 
        /// </summary>
        [DataMember(Name = "Comments", EmitDefaultValue = false, IsRequired = false)]
        public string Comments { get; set; }


    }
}
