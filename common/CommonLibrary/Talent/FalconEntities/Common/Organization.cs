//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Talent.FalconEntities.Common
{
    using System.Runtime.Serialization;
    using Common.DocumentDB.Contracts;

    /// <summary>
    /// The Organization entity.
    /// </summary>
    [DataContract]
    public class Organization : DocDbEntity
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
