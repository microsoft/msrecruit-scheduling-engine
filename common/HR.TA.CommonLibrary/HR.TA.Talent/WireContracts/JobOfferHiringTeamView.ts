//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobOfferHiringTeamView {
    jobApplicationID?: string;
    jobOfferID?: string;
    jobOfferStatus?: JobOfferStatus;
    jobOfferStatusReason?: JobOfferStatusReason;
    jobOfferPublishDate?: Date;
}
