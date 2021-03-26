//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.EnumSetModel.SchedulingService
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Email Content Type enums
    /// </summary>
    [DataContract]
    public enum EmailContentType
    {
        /// <summary>
        /// Subject Section
        /// </summary>
        Subject = 0,

        /// <summary>
        /// Header Section
        /// </summary>
        Header = 1,

        /// <summary>
        /// Actual Email Body Section - Greeting and Main Body Content
        /// </summary>
        EmailBody = 2,

        /// <summary>
        /// Closing of email
        /// </summary>
        Closing = 3,

        /// <summary>
        /// Footer Section
        /// </summary>
        Footer = 4,
    }
}
