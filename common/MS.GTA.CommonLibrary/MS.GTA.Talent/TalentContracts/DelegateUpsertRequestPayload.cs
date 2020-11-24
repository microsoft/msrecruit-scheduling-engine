//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class representing a list of delegates to upsert
    /// </summary>
    [DataContract]
    public class DelegateUpsertRequestPayload
    {
        /// <summary>
        /// List of users to add as delegates
        /// </summary>
        [DataMember(Name = "delegates", IsRequired = true, EmitDefaultValue = false)]
        public IList<Delegate> Delegates { get; set; }
    }
}
