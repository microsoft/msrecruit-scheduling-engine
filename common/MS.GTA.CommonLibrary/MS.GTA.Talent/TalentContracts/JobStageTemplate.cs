//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobStageTemplate.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;

    /// <summary>
    /// The job data contract.
    /// </summary>
    [DataContract]
    public class JobStageTemplate : TalentBaseContract
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
        
        /// <summary>Gets or sets a value of ordinal.</summary>
        [DataMember(Name = "ordinal", IsRequired = false)]
        public long Ordinal { get; set; }
    }
}
