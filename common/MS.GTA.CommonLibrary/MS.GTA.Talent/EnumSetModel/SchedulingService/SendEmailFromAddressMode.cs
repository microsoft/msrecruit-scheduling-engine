//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.EnumSetModel.SchedulingService
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Send Email From Address Mode
    /// </summary>
    [DataContract]
    public enum SendEmailFromAddressMode
    {
        /// <summary>
        /// System Account
        /// </summary>
        SystemAccount,

        /// <summary>
        /// Current User
        /// </summary>
        CurrentUser,
    }
}
