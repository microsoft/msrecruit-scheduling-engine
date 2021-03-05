//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The template settings contract.
    /// </summary>
    [DataContract]
    public class TemplateSettings
    {
        /// <summary>
        /// Gets or sets flag to disable template creation process.
        /// </summary>
        [DataMember(Name = "disableTemplateCreation", IsRequired = false, EmitDefaultValue = true)]
        public bool DisableTemplateCreation { get; set; }

        /// <summary>
        /// Gets or sets default template Ids.
        /// </summary>
        [DataMember(Name = "defaultTemplateIds", IsRequired = false, EmitDefaultValue = false)]
        public List<string> DefaultTemplateIds { get; set; }

        /// <summary>
        /// Gets or sets modified by .
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false, EmitDefaultValue = false)]
        public Person ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets modified by .
        /// </summary>
        [DataMember(Name = "modifiedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime ModifiedDateTime { get; set; }
    }
}
