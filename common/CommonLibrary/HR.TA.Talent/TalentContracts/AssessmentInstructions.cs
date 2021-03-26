//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessments provided by external assessment providers.
    /// </summary>
    [DataContract]
    public class AssessmentInstructions
    {
        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        [DataMember(Name = nameof(Introduction), IsRequired = false, EmitDefaultValue = false)]
        public string Introduction { get; set; }

        /// <summary>
        /// Gets or sets the ending.
        /// </summary>
        [DataMember(Name = nameof(Ending), IsRequired = false, EmitDefaultValue = false)]
        public string Ending { get; set; }
    }
}
