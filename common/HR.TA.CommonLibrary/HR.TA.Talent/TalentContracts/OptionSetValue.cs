//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OptionSetValue
    {
        /// <summary>The integer value.</summary>
        [DataMember(Name = "value", IsRequired = false)]
        public int? Value { get; set; }

        /// <summary>The translated label for the value.</summary>
        [DataMember(Name = "label", IsRequired = false)]
        public string Label { get; set; }

        /// <summary>The name of the option set - used to get the list of other values.</summary>
        [DataMember(Name = "optionset", IsRequired = false)]
        public string OptionSet { get; set; }
    }
}
