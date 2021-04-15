//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Interview activity.
    /// </summary>
    [DataContract]
    public class InterviewConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow addition of participants outside of loop.
        /// </summary>
        [DataMember(Name = "allowParticipantsOutsideOfLoop", IsRequired = false, EmitDefaultValue = false)]
        public bool AllowParticipantsOutsideOfLoop { get; set; }
    }
}
