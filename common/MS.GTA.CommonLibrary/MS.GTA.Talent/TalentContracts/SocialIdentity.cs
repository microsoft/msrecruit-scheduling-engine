//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Social Identity data contract.
    /// </summary>
    [DataContract]
    public class SocialIdentity
    {
        /// <summary>
        /// Gets or sets the applicant.
        /// </summary>
        [DataMember(Name = "applicant", IsRequired = false, EmitDefaultValue = false)]
        public Applicant Applicant { get; set; }

        /// <summary>
        /// Gets or sets the network provider.
        /// </summary>
        [DataMember(Name = "provider", IsRequired = false)]
        public SocialNetworkProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the provider member id.
        /// </summary>
        [DataMember(Name = "providerMemberId", IsRequired = false, EmitDefaultValue = false)]
        public string ProviderMemberId { get; set; }
    }
}
