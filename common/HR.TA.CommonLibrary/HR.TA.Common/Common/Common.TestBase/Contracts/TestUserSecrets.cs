//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TestBase
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TestUserSecrets
    {
        /// <summary>
        /// Gets or sets test user names.
        /// </summary>
        [DataMember(Name = "testUsers")]
        public string TestUsers { get; set; }

        /// <summary>
        /// Gets or sets test user passowrds.
        /// </summary>
        [DataMember(Name = "testUserPasswords")]
        public string TestUserPasswords { get; set; }
    }
}
