//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The feature setting data contract.
    /// </summary>
    [DataContract]
    public class FeatureSettings
    {
        /// <summary>
        /// Gets or sets feature.
        /// </summary>
        [DataMember(Name = "feature", IsRequired = false, EmitDefaultValue = true)]
        public Feature Feature { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether feature is enabled or not.
        /// </summary>
        [DataMember(Name = "isEnabled", IsRequired = false, EmitDefaultValue = true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets last modified by.
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false, EmitDefaultValue = false)]
        public Person ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date time.
        /// </summary>
        [DataMember(Name = "modifiedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime ModifiedDateTime { get; set; }
    }
}
