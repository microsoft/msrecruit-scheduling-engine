//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantAttachment {
    id?: string;
    documentType?: CandidateAttachmentDocumentType;
    type?: CandidateAttachmentType;
    name?: string;
    description?: string;
    reference?: string;
    userAction?: UserAction;
    isJobApplicationAttachment?: boolean;
    uploadedBy?: Person;
    uploadedDateTime?: Date;
}
