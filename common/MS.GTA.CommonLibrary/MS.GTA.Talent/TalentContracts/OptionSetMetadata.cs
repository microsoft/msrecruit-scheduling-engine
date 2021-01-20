//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class OptionSetMetadata
    {
        /// <summary>The values.</summary>
        [DataMember(Name = "values", IsRequired = true)]
        public IList<OptionSetValue> Values { get; set; }
    }
}
