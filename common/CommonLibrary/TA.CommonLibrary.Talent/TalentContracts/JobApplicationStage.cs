//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using TA.CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The Job application stage data contract.
    /// </summary>
    [DataContract]
    public class JobApplicationStage
    {
        /// <summary>Gets or sets the stage.</summary>
        [DataMember(Name = "stage", IsRequired = false)]
        public JobStage Stage { get; set; }

        /// <summary>Gets or sets the order.</summary>
        [DataMember(Name = "order", IsRequired = false)]
        public long? Order { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }
    }
}
