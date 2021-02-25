//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ConferenceRequest {
    startTime: Date;
    endTime: Date;
    subject: string;
    participantEmailAddresses: string[];
}
