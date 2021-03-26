//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EnvironmentSettings {
    id: string;
    templateSetting?: TemplateSettings;
    featureSettings?: FeatureSettings[];
    integrationSettings?: { [key: string]: IntegrationSetting; };
    offerSettings?: OfferSettings;
    eSignSettings?: ESignSettings;
    emailTemplateSettings?: EmailTemplateSettings;
    isPremium?: boolean;
}
