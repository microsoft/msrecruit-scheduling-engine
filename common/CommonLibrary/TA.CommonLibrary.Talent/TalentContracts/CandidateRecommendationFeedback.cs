//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The candidate  recommendation feedback data contract.
    /// </summary>
    [DataContract]
    public class CandidateRecommendationFeedback
    {
        /// <summary>Gets or sets applicantId.</summary>
        [DataMember(Name = "applicantId", IsRequired = false)]
        public string ApplicantId { get; set; }

        /// <summary> Gets or sets a value indicating whether the user is interested in the candidate. </summary>
        [DataMember(Name = "interested", IsRequired = false)]
        public bool Interested { get; set; }

        /// <summary>Gets or sets a value indicating whether the candidate experience is disliked by the user.</summary>
        [DataMember(Name = "experience", IsRequired = false)]
        public bool Experience { get; set; }

        /// <summary>Gets or sets a value indicating whether the candidate skills is disliked by the user.</summary>
        [DataMember(Name = "skills", IsRequired = false)]
        public bool Skills { get; set; }

        /// <summary> Gets or sets a value indicating whether the candidate education is disliked by the user. </summary>
        [DataMember(Name = "education", IsRequired = false)]
        public bool Education { get; set; }

        /// <summary> Gets or sets a value indicating whether there are other reasons for not liking the candidate recommendation. </summary>
        [DataMember(Name = "other", IsRequired = false)]
        public bool Other { get; set; }
    }
}
