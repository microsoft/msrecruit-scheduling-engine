//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.V2;
    using Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Template Participant update model
    /// </summary>
    [DataContract]
    public class TemplateParticipant : AadUser
    {
        /// <summary>
        /// Gets or sets Role of template participants
        /// </summary>
        [DataMember(Name = "role")]
        public OfferTemplateRole Role { get; set; }
    }
}
