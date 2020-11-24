//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="UserEmailPasswordSecret.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase
{
    /// <summary>
    /// User Name Password Secret
    /// </summary>
    public class UserEmailPasswordSecret
    {
        /// <summary>
        /// User email
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Primary Password
        /// </summary>
        public string PrimaryPassword { get; set; }
        /// <summary>
        /// Secondary Password
        /// </summary>
        public string SecondaryPassword { get; set; }
    }
}
