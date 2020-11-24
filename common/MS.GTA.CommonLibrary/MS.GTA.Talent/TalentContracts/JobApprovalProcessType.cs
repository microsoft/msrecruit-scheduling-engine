﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum that tells about type of job approval process.
    /// </summary>
    [DataContract]
    public enum JobApprovalProcessType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Default
        /// </summary>
        Default = 1,
    }
}
