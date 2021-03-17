//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.WebNotifications.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="ParticipantData"/> stores participant information.
    /// </summary>
    [DataContract]
    public class ParticipantData
    {
        /// <summary>
        /// Gets or sets the participant object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [DataMember(Name = "objectIdentifier")]
        public string ObjectIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the participant name.
        /// </summary>
        /// <value>
        /// The participant name.
        /// </value>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the participant email.
        /// </summary>
        /// <value>
        /// The participant email.
        /// </value>
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
