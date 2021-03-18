//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EmailTemplateSettings {
    emailTemplateFooterText?: string;
    emailTemplatePrivacyPolicyLink?: string;
    emailTemplateTermsAndConditionsLink?: string;
    shouldDisableEmailEdits?: boolean;
    emailTemplateHeaderImgUrl?: string;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}
