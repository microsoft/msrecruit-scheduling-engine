//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobParticipant.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.TeamsIntegration
{
    using MS.GTA.TalentEntities.Enum;
    using System.Runtime.Serialization;

    /// <summary>
    /// Job Participant.
    /// </summary>
    [DataContract]
    public class JobParticipant
    {
        /// <summary>
        /// Gets or sets Full name.
        /// </summary>
        [DataMember(Name = "FullName", EmitDefaultValue = false, IsRequired = false)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Alias.
        /// </summary>
        [DataMember(Name = "Alias", EmitDefaultValue = false, IsRequired = false)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets EmailPrimary.
        /// </summary>
        [DataMember(Name = "EmailPrimary", EmitDefaultValue = false, IsRequired = false)]
        public string EmailPrimary { get; set; }

        /// <summary>
        /// Gets or sets OfficeGraphIdentifier.
        /// </summary>
        [DataMember(Name = "OfficeGraphIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string OfficeGraphIdentifier { get; set; }

        /// <summary>
        /// Gets or sets TeamsIdentifier.
        /// </summary>
        [DataMember(Name = "TeamsIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string TeamsIdentifier { get; set; }

        /// <summary>
        /// Gets or sets Alias.
        /// </summary>
        [DataMember(Name = "Role", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? Role { get; set; }
    }
}
