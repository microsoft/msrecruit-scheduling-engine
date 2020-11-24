//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicantTag.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The Applicant Tag data contract.
    /// </summary>
    [DataContract]
    public class ApplicantTag
    {
        /// <summary>
        /// Gets or sets tag id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets tag name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }        
    }
}
