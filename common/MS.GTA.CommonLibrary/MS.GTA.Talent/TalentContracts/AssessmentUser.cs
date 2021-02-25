//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessment user.
    /// </summary>
    [DataContract]
    public class AssessmentUser
    {
        /// <summary>
        /// Gets or sets the User Id.
        /// </summary>
        [DataMember(Name = "userId", IsRequired = false, EmitDefaultValue = false)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [DataMember(Name = "role", IsRequired = false, EmitDefaultValue = false)]
        public int Role { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
