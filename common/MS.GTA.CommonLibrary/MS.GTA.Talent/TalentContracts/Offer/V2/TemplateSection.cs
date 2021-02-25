//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Template Section
    /// </summary>
    [DataContract]
    public class TemplateSection
    {
        /// <summary>
        /// Gets or sets Section Id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Section Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Section Color.
        /// </summary>
        [DataMember(Name = "color", IsRequired = true)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets Ordinal Number.
        /// </summary>
        [DataMember(Name = "ordinalNumber", IsRequired = false)]
        public int OrdinalNumber { get; set; }

        /// <summary>
        /// Gets or sets tokens.
        /// </summary>
        [DataMember(Name = "tokens", IsRequired = false)]
        public IList<TemplateToken> Tokens { get; set; }
    }
}
