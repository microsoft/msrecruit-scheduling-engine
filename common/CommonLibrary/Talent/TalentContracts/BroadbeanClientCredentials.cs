//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Credentials for the client
    /// </summary>
    [DataContract]
    public class BroadbeanClientCredentials
    {
        /// <summary>
        /// Broadbean username
        /// </summary>
        [DataMember(Name = "username", IsRequired = true)]
        public string Username { get; set; }

        /// <summary>
        /// Broadbean clientId
        /// </summary>
        [DataMember(Name = "clientId", IsRequired = true)]
        public string ClientId { get; set; }

        /// <summary>
        /// Broadbean encryption token
        /// </summary>
        [DataMember(Name = "encryptionToken", IsRequired = true)]
        public string EncryptionToken { get; set; }
    }
}
