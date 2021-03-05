//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Integration.Contracts
{
    using System;

    /// <summary>
    /// MSIntFeedback
    /// </summary>
    public class MSIntFeedback
    {
        public string Comment { get; set; }

        public string Status { get; set; }

        public string StatusReason { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? RespondDate { get; set; }

        public bool? IsSubmitted { get; set; }

        public string SubmittedBy { get; set; }
    }
}
