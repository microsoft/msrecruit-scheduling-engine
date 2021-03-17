//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessment project contributors.
    /// </summary>
    [DataContract]
    public class AssessmentProjectContributors
    {
        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        [DataMember(Name = "projectId", IsRequired = false, EmitDefaultValue = false)]
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the add contributors.
        /// </summary>
        [DataMember(Name = "addContributors", IsRequired = false, EmitDefaultValue = false)]
        public IList<AssessmentUser> AddContributors { get; set; }

        /// <summary>
        /// Gets or sets the remove contributors.
        /// </summary>
        [DataMember(Name = "removeContributors", IsRequired = false, EmitDefaultValue = false)]
        public IList<AssessmentUser> RemoveContributors { get; set; }
    }
}
