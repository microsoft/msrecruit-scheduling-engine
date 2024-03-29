﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>OAUTH token information</summary>
    [DataContract]
    public class OAuthToken
    {
        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        [DataMember(Name = "token_type", IsRequired = true)]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets expiration time in seconds
        /// </summary>
        [DataMember(Name = "expires_in", IsRequired = true)]
        public int ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets assessment provider OAUTH authentication token.
        /// </summary>
        [DataMember(Name = "access_token", IsRequired = true)]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets Id token.
        /// </summary>
        [DataMember(Name = "id_token", IsRequired = true)]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets assessment provider refresh token.
        /// </summary>
        [DataMember(Name = "refresh_token", IsRequired = true)]
        public string RefreshToken { get; set; }
    }
}
