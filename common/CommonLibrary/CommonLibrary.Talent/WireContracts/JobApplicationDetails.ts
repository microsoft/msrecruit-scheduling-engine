//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantAssessmentReport {
    title: string;
    assessmentURL?: string;
    externalAssessmentReportID?: string;
    providerKey?: string;
    assessmentStatus?: AssessmentStatus;
    dateOrdered?: Date;
    dateCompleted?: Date;
    stageOrdinal?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
}

export interface JobApplicationInterview {
    InterviewerName: string;
    LinkedinIdentity: string;
    StartDate: Date;
    EndDate: Date;
    stageOrdinal?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
}

export interface JobApplicationDetails extends TalentBaseContract {
    ApplicationId: string;
    TenantId: string;
    CompanyName: string;
    PositionLocation: string;
    PositionTitle: string;
    JobDescription: string;
    JobPostLink?: string;
    DateApplied: Date;
    Status: JobApplicationStatus;
    ExternalStatus?: string;
    ExternalSource?: JobApplicationExternalSource;
    StatusReason?: JobApplicationStatusReason;
    RejectionReason?: OptionSetValue;
    CurrentJobStage?: JobStage;
    CurrentApplicationStage?: ApplicationStage;
    Interviews: JobApplicationInterview[];
    ApplicationSchedules?: ApplicationSchedule[];
    ApplicantAttachments?: ApplicantAttachment[];
    ApplicantAssessments?: ApplicantAssessmentReport[];
    ApplicationStages?: ApplicationStage[];
    candidatePersonalDetails?: CandidatePersonalDetails[];
}
