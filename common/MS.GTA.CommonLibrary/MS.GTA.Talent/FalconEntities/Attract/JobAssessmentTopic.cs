//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class JobAssessmentTopic
    {
        [DataMember(Name = "JobAssessmentTopicID", EmitDefaultValue = false, IsRequired = false)]
        public string JobAssessmentTopicID { get; set; }

        [DataMember(Name = "Topic", EmitDefaultValue = false, IsRequired = false)]
        public string Topic { get; set; }

        [DataMember(Name = "RatingType", EmitDefaultValue = false, IsRequired = false)]
        public JobAssessmentTopicRatingType? RatingType { get; set; }
    }
}
