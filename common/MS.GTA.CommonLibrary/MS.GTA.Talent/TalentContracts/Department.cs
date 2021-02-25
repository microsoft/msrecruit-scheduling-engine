//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Department.
    /// </summary>
    [DataContract]
    public class Department
    {
        /// <summary>Gets or sets department id</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets description</summary>
        [DataMember(Name = "description", IsRequired = false)]
        public string Descripition { get; set; }

        /// <summary>Gets or sets name</summary>
        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }

        /// <summary>Gets or sets parent department id</summary>
        [DataMember(Name = "parentDepartment", IsRequired = false)]
        public Department ParentDepartment { get; set; }

        /// <summary>Gets or sets job positions</summary>
        [DataMember(Name = "jobPositions", IsRequired = false, EmitDefaultValue = false)]
        public IList<JobOpeningPosition> JobOpeningPositions { get; set; }
    }
}