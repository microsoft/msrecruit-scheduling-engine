//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TestBase
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
