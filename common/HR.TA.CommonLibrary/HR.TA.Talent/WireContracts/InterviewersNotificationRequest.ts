//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface InterviewersNotificationRequest {
    jobApplicationId: string;
    interviewers: GraphPerson[];
}
