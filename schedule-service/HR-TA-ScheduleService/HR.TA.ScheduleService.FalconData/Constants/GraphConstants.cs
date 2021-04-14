//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace HR.TA.ScheduleService.FalconData.Constants
{
    /// <summary>
    /// Graph constants
    /// </summary>
    public static class GraphConstants
    {
        /// <summary>The tentative message type</summary>
        public const string TentativeMessageType = "meetingtenativelyaccepted";

        /// <summary>The accept message type</summary>
        public const string AcceptMessageType = "meetingaccepted";

        /// <summary>The decline message type</summary>
        public const string DeclineMessageType = "meetingdeclined";
    }
}
