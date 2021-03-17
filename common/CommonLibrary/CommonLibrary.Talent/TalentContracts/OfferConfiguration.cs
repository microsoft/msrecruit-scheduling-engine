//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Offer activity.
    /// </summary>
    [DataContract]
    public class OfferConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether hiring manager is allowed to prepare offer.
        /// </summary>
        [DataMember(Name = "allowHiringManagerToPrepareOffer", IsRequired = false, EmitDefaultValue = false)]
        public bool? AllowHiringManagerToPrepareOffer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether positions can be reused within a given job or not.
        /// </summary>
        [DataMember(Name = "allowPositionReuseWithinJob", IsRequired = false, EmitDefaultValue = false)]
        public bool? AllowPositionReuseWithinJob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to launch onboard app.
        /// </summary>
        [DataMember(Name = "launchOnboardApp", IsRequired = false, EmitDefaultValue = false)]
        public bool LaunchOnboardApp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to provide manual record.
        /// </summary>
        [DataMember(Name = "manuallyRecordStatus", IsRequired = false, EmitDefaultValue = false)]
        public bool ManuallyRecordStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to send mail to candidate.
        /// </summary>
        [DataMember(Name = "sendMailToCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool? SendMailToCandidate { get; set; }
    }
}
