﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ProjectResponse.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for Koru Project response.
    /// </summary>
    [DataContract]
    public class ProjectResponse
    {
        /// <summary>
        /// Gets or sets project id.
        /// </summary>
        [DataMember(Name = "projectId", IsRequired = false, EmitDefaultValue = false)]
        public string ProjectId { get; set; }
        
        /// <summary>
        /// Gets or sets project access token.
        /// </summary>
        [DataMember(Name = "resourceToken", IsRequired = false, EmitDefaultValue = false)]
        public string ResourceToken { get; set; }
    }
}
