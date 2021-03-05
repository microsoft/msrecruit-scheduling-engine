//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
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
