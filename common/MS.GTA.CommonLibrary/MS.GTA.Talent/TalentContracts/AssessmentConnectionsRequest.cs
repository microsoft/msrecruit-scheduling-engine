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
    /// Assessment connection missing request contract.
    /// </summary>
    [DataContract]
    public class AssessmentConnectionsRequest
    {
        /// <summary>
        /// Gets or sets Job Opening Id.
        /// </summary>
        [DataMember(Name = "jobOpeningId", IsRequired = false)]
        public string JobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets Assessment providers.
        /// </summary>
        [DataMember(Name = "providers", IsRequired = false)]
        public IEnumerable<AssessmentProvider> Providers { get; set; }
    }
}
