//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset
{
    public enum JobOfferApprovalStatus
    {
        NotStarted = 0,
        Approved = 1,
        Sendback = 2,
        SentForReview = 3,
        WaitingForReview = 4,
        Skipped = 5
    }
}