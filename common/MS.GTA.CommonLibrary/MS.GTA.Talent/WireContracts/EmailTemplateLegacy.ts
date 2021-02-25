//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EmailTemplateLegacy {
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
    primaryEmailRecipients?: GraphPerson[];
    emailContent?: EmailContent[];
    emailTokenList?: EmailTemplateTokens[];
    fromAddressMode?: SendEmailFromAddressMode;
}
