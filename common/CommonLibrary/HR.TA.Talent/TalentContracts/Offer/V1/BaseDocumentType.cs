//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Document Type
    /// </summary>
    [DataContract]
    public class BaseDocumentType
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets document type property
        /// </summary>
        [DataMember(Name = "DocumentType", IsRequired = true)]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets document type property
        /// </summary>
        [DataMember(Name = "PartitionKey", IsRequired = true)]
        public string PartitionKey { get; set; }
    }
}
