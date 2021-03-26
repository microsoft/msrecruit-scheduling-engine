//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FeedbackConfiguration {
    inheritFromHiringTeam?: boolean;
    viewFeedbackBeforeSubmitting?: boolean;
    editFeedbackAfterSubmitting?: boolean;
    feedbackReminderDelay?: FeedbackReminderDelay;
}

export enum FeedbackReminderDelay {
    None = 0,
    HalfDay = 1,
    OneDay = 2,
    TwoDays = 3,
    ThreeDays = 4,
    OneWeek = 5,
}
