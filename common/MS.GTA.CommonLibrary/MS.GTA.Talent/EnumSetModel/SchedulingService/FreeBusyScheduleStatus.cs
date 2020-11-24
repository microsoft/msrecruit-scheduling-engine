//----------------------------------------------------------------------------
// <copyright file="FreeBusyScheduleStatus.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.EnumSetModel.SchedulingService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// FreeBusy Schedule time slot status
    /// </summary>
    [DataContract]
    public enum FreeBusyScheduleStatus
    {
        /// <summary>
        /// free
        /// </summary>
        Free = 0,

        /// <summary>
        /// tentative
        /// </summary>
        Tentative = 1,

        /// <summary>
        /// busy
        /// </summary>
        Busy = 2,

        /// <summary>
        /// oof
        /// </summary>
        Oof = 3,

        /// <summary>
        /// working else where
        /// </summary>
        WorkingElsewhere = 4,

        /// <summary>
        /// unknown
        /// </summary>
        Unknown = 5,

        /// <summary>
        /// non working hour
        /// </summary>
        NonWorkingHour = 6
    }
}
