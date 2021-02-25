//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface SkypeOnlineMeetingExtension {
    id: string;
    type: string;
    onlineMeetingConfLink: string;
    onlineMeetingExternalLink: string;
    onlineMeetingInternalLink: string;
    ucMeetingSetting: string;
    ucInband: string;
    ucCapabilities: string;
    participantPassCode: string;
    tollNumber: string;
    tollFreeNumber: string;
    _links: SkypeLinks;
}
