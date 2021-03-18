//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The Organization entity.
    /// </summary>
    [DataContract]
    public class Organization
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "Organization";

        /// <summary>
        /// Gets or sets the person's display name.
        /// </summary>
        [DataMember(Name = "displayName", IsRequired = false)]
        public string DisplayName { get; set; }
    }
}
