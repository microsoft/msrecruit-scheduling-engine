//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for feedback state
    /// </summary>
    [DataContract]
    public enum FeedbackAction
    {
        /// <summary>
        /// Save feedback
        /// </summary>
        Save,

        /// <summary>
        /// Submit feedback
        /// </summary>
        Submit
    }
}
