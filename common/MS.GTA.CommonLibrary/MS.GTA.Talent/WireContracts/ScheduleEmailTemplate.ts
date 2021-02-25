//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleEmailTemplate {
    id?: string;
    tenantId?: string;
    userObjectId?: string;
    templateName?: string;
    templateType?: TemplateType;
    isDefault?: boolean;
    isAutosent?: boolean;
    isTentative?: boolean;
    ccEmailAddressRoles?: JobParticipantRole[];
    ccEmailAddressList?: string[];
    bccEmailAddressRoles?: JobParticipantRole[];
    bccEmailAddressList?: string[];
    primaryEmailRecipients?: string[];
    subject?: string;
    emailContent?: string;
    emailTokenList?: EmailTemplateTokens[];
    fromAddressMode?: SendEmailFromAddressMode;
}
