// <copyright file="TemplateParticipant.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.V2;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

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
