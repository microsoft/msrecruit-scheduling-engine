//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for Add Koru candidate to project.
    /// </summary>
    [DataContract]
    public class AddCandidateProjectResponse
    {
        /// <summary>
        /// Gets or sets the Link Url for assessment.
        /// </summary>
        [DataMember(Name = "linkUrl", IsRequired = false, EmitDefaultValue = false)]
        public string LinkUrl { get; set; }
    }
}
