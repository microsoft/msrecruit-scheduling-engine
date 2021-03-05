//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>
    /// Contract for Job closing. 
    /// </summary>
    [DataContract]
    public class JobClosing
    {
        /// <summary>
        /// Gets or sets Job opening status 
        /// </summary>
        [DataMember(Name = "jobOpeningStatus")]
        public JobOpeningStatus JobOpeningStatus { get; set; }

        /// <summary>
        /// Gets or sets Job opening status reason
        /// </summary>
        [DataMember(Name = "jobOpeningStatusReason")]
        public JobOpeningStatusReason JobOpeningStatusReason { get; set; }

        /// <summary>
        /// Gets or sets Job opening external status 
        /// </summary>
        [DataMember(Name = "jobOpeningExternalStatus", IsRequired = false)]
        public string JobOpeningExternalStatus { get; set; }

        /// <summary>
        /// Gets or sets comment. 
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets Job application Ids for offered accepted job applicant.
        /// </summary>
        [DataMember(Name = "offerAcceptedJobApplicationIds", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> OfferAcceptedJobApplicationIds { get; set; }
    }
}
