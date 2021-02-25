//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface OAuthToken {
    token_type: string;
    expires_in: number;
    access_token: string;
    id_token: string;
    refresh_token: string;
}
