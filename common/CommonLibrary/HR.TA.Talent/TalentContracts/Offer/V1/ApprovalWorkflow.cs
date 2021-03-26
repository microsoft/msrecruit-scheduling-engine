//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Approval Workflow
    /// </summary>
    public class ApprovalWorkflow
    {
        /// <summary>
        /// Gets or sets a value indicating whether approval process is sequential
        /// </summary>
        public bool IsSequential { get; set; }
    }
}
