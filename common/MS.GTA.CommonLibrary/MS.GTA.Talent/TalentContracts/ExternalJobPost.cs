//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalJobPost.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The job data contract.
    /// </summary>
    [DataContract]
    public class ExternalJobPost : TalentBaseContract
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets uri.
        /// </summary>
        [DataMember(Name = "uri", IsRequired = false)]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets supplier.
        /// </summary>
        [DataMember(Name = "supplier", IsRequired = false)]
        public JobPostSupplier Supplier { get; set; }

        /// <summary>
        /// Gets or sets supplier.
        /// </summary>
        [DataMember(Name = "supplierName", IsRequired = false)]
        public string SupplierName { get; set; }

        /// <summary>
        /// Gets or sets isRepostAvailable.
        /// </summary>
        [DataMember(Name = "isRepostAvailable", IsRequired = false)]
        public bool IsRepostAvailable { get; set; }

        /// <summary>
        /// Gets or sets user action.
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false)]
        public UserAction UserAction { get; set; }

        /// <summary>
        /// Gets or sets ExtendedAttributes.
        /// </summary>
        [DataMember(Name = "extendedAttributes", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, string> ExtendedAttributes { get; set; }
    }
}
