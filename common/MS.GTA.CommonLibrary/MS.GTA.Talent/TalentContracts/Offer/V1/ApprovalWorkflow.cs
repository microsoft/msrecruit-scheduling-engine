﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApprovalWorkflow.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
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
