//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Web.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract for Authorized Token Type
    /// </summary>
    [DataContract]
    public enum AuthorizedTokenType
    {
        /// <summary>UserToken</summary>
        UserToken = 0,

        /// <summary>ApplicationToken</summary>
        ApplicationToken = 1,

        /// <summary>UserOrApplicationToken</summary>
        UserOrApplicationToken = 2,
    }
}
