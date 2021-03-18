//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;
    using CommonLibrary.Common.OfferManagement.Contracts.V2;
    using CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

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
