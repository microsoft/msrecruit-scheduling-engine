//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="DocDbEntity.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB.Contracts
{
    using System;
    using System.Runtime.Serialization;

    using MS.GTA.Common.DocumentDB.Attributes;
    using MS.GTA.Common.DocumentDB.Enums;

    /// <summary>The Document DB entity.</summary>
    [DataContract]
    [PartitionKeyPath("/Type")]
    [BackfillDBAndCollection(false)]
    public abstract class DocDbEntity
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the type.</summary>
        public string Type { get; set; }

        /// <summary>Gets or sets the OID that the document was created by.</summary>
        public string CreatedBy { get; set; }

        /// <summary>Gets or sets the OID that the document was updated by.</summary>
        public string UpdatedBy { get; set; }

        /// <summary>Gets or sets the created at.</summary>
        public long CreatedAt { get; set; }

        /// <summary>Gets or sets the updated at.</summary>
        public long UpdatedAt { get; set; }

        /// <summary>Gets or sets the created at.</summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>Gets or sets the updated at.</summary>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary> Gets or sets the External Source</summary>
        [DataMember(Name = "ExternalSource", EmitDefaultValue = false, IsRequired = false)]
        public ExternalSource ExternalSource { get; set; }

        /// <summary> Gets or sets the External Source updated date time</summary>
        [DataMember(Name = "ExternalSourceUpdatedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime ExternalSourceUpdatedDateTime { get; set; }
    }
}
