//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ConferenceInfo {
    id: string;
    subject: string;
    joinUrl: string;
    audio: AudioInfo;
    admitUsers: string;
    joinInfo: string;
    provider: ConferenceProvider;
}
