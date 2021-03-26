//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The integration setting data contract.
    /// </summary>
    [DataContract]
    public class IntegrationSetting
    {
        /// <summary>
        /// Gets or sets the integration value.
        /// </summary>
        [DataMember(Name = "value", IsRequired = false, EmitDefaultValue = true)]
        public string IntegrationValue { get; set; }

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
