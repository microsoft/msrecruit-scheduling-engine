//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The assessment result as received from the Koru external assessment provider.
    /// </summary>
    [DataContract]
    public class KoruAssessmentResultPayload
    {
        [DataMember(Name = "extraData", IsRequired = false, EmitDefaultValue = false)]
        public object ExtraData { get; set; }

        [DataMember(Name = "webhookUrl", IsRequired = false, EmitDefaultValue = false)]
        public string WebhookUrl { get; set; }

        [DataMember(Name = "processed", IsRequired = false, EmitDefaultValue = false)]
        public bool Processed { get; set; }

        [DataMember(Name = "tenantId", IsRequired = false, EmitDefaultValue = false)]
        public string TenantId { get; set; }

        [DataMember(Name = "scores", IsRequired = false, EmitDefaultValue = false)]
        public string Scores { get; set; }

        [DataMember(Name = "candidate", IsRequired = false, EmitDefaultValue = false)]
        public string Candidate { get; set; }

        [DataMember(Name = "inProgress", IsRequired = false, EmitDefaultValue = false)]
        public bool InProgress { get; set; }

        [DataMember(Name = "scored", IsRequired = false, EmitDefaultValue = false)]
        public bool Scored { get; set; }

        [DataMember(Name = "uuid", IsRequired = false, EmitDefaultValue = false)]
        public string Uuid { get; set; }

        [DataMember(Name = "initialized", IsRequired = false, EmitDefaultValue = false)]
        public bool Initialized { get; set; }

        [DataMember(Name = "projectUuid", IsRequired = false, EmitDefaultValue = false)]
        public string ProjectUuid { get; set; }

        [DataMember(Name = "profileUrl", IsRequired = false, EmitDefaultValue = false)]
        public string ProfileUrl { get; set; }
    }

    /// <summary>
    /// The assessment result data.
    /// </summary>
    [DataContract]
    public class KoruAssessmentResultData
    {
        [DataMember(Name = "curiosity", IsRequired = false, EmitDefaultValue = false)]
        public double Curiosity { get; set; }

        [DataMember(Name = "grit", IsRequired = false, EmitDefaultValue = false)]
        public double Grit { get; set; }

        [DataMember(Name = "impact", IsRequired = false, EmitDefaultValue = false)]
        public double Impact { get; set; }

        [DataMember(Name = "ownership", IsRequired = false, EmitDefaultValue = false)]
        public double Ownership { get; set; }

        [DataMember(Name = "polish", IsRequired = false, EmitDefaultValue = false)]
        public double Polish { get; set; }

        [DataMember(Name = "rigor", IsRequired = false, EmitDefaultValue = false)]
        public double Rigor { get; set; }

        [DataMember(Name = "teamwork", IsRequired = false, EmitDefaultValue = false)]
        public double Teamwork { get; set; }

        [DataMember(Name = "profileUrl", IsRequired = false, EmitDefaultValue = false)]
        public string ProfileUrl { get; set; }
    }
}
