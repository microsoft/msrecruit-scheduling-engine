//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------

namespace Common.Provisioning.Entities.XrmEntities.Optionset
{
    public enum JobOfferApprovalStatusReason
    {
        NotStarted = 0,
        Approved = 1,
        Sendback = 2,
        SentForReview = 3,
        WaitingForReview = 4,
        Skipped = 5
    }
}
