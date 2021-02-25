//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EmailNotificationRequest {
    jobApplicationId: string;
    emailBody: string;
    emailFooter?: string;
    subject: string;
    mailTo: GraphPerson[];
    mailCC?: GraphPerson[];
}
