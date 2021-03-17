//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Optionset
{
    public enum JobOfferStatusReason
    {
        New = 0,
        Draft = 1,
        Review = 2,
        Approved = 3,
        Published = 4,
        Accepted = 5,
        WithdrawnCandidateDispositioned = 6,
        NeedRevision = 7,
        WithdrawnOther = 8,
        Declined = 9,
        Closed = 10,
        Expired = 11,
    }
}
