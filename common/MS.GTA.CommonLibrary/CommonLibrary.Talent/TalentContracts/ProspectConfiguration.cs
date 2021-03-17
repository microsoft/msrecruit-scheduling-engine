//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Prospect activity.
    /// </summary>
    [DataContract]
    public class ProspectConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether hiring managers have access to prospects
        /// </summary>
        [DataMember(Name = "allowHiringManager", IsRequired = false, EmitDefaultValue = false)]
        public bool AllowHiringManager { get; set; }
    }
}
