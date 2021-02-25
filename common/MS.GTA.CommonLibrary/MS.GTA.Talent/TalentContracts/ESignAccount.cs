//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for ESign Account (This will be moved to Key Vault)
    /// </summary>
    [DataContract]
    public class ESignAccount
    {
        /// <summary>
        /// Document type.
        /// </summary>
        public const string DocumentType = "esign-account";

        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets ESign Type
        /// </summary>
        [DataMember(Name = "eSignType", IsRequired = false)]
        public ESignType ESignType { get; set; }

        /// <summary>
        /// Gets or sets Client Secret Key
        /// </summary>
        [DataMember(Name = "secret", IsRequired = false)]
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets Client Integration Key
        /// </summary>
        [DataMember(Name = "integrationKey", IsRequired = false)]
        public string IntegrationKey { get; set; }
    }
}
