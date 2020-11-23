//----------------------------------------------------------------------------
// <copyright file="GraphConstants.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace MS.GTA.ScheduleService.FalconData.Constants
{
    /// <summary>
    /// Graph constants
    /// </summary>
    public static class GraphConstants
    {
        /// <summary>The tentative message type</summary>
        public const string TentativeMessageType = "meetingTenativelyAccepted";

        /// <summary>The accept message type</summary>
        public const string AcceptMessageType = "meetingAccepted";

        /// <summary>The decline message type</summary>
        public const string DeclineMessageType = "meetingDeclined";
    }
}
