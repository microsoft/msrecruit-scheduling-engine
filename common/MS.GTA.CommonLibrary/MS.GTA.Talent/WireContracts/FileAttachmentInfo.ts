//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FileAttachmentInfo {
    fileId: string;
    name: string;
    blobURI: string;
    fileLabel?: string;
    description?: string;
    documentType?: string;
    fileSize?: number;
}
