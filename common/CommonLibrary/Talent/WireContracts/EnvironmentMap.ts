//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EnvironmentMap {
    id?: string;
    autoNumber?: string;
    displayName?: string;
    alias?: string;
    brandingImages?: BrandingImage[];
    companyHeadquarters?: string;
    companyWebpage?: string;
    contactEmail?: string;
    phoneNumber?: string;
    environmentId?: string;
    tenantId?: string;
    privacyLink?: string;
    tosLink?: string;
    termsAndConditionsLink?: string;
    termsAndConditionsText?: string;
}
