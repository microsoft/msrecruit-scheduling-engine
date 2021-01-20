//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

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
