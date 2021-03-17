//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobOffer {
    url: string;
    status?: JobOfferStatus;
    jobOfferStatusReason?: JobOfferStatusReason;
    offerDate?: Date;
}
