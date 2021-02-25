//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EmailTemplate {
    id?: string;
    templateName: string;
    appName: string;
    templateType: string;
    isGlobal?: boolean;
    to?: string[];
    cc?: string[];
    additionalCc?: string[];
    bcc?: string[];
    subject?: string;
    header?: string;
    body?: string;
    attachments?: FileAttachmentRequest;
    closing?: string;
    footer?: string;
    isDefault?: boolean;
    isAutosent?: boolean;
    creator?: string;
    language?: string;
}
