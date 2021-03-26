//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{

    using System.Runtime.Serialization;
    using HR.TA.Common.Contracts;

    /// <summary>
    /// The talent pool contract.
    /// </summary>
    [DataContract]
    public class TalentSource : TalentBaseContract
    {
        [DataMember(Name = "id", EmitDefaultValue = false, IsRequired = false)]
        public string Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "domain", EmitDefaultValue = false, IsRequired = false)]
        public string Domain { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        /// <summary>Gets or sets the rejection reason</summary>
        [DataMember(Name = "talentSourceCategory", IsRequired = false)]
        public OptionSetValue TalentSourceCategory { get; set; }

        [DataMember(Name = "referalReference", EmitDefaultValue = false, IsRequired = false)]
        public Person ReferalReference { get; set; }

    }
}
