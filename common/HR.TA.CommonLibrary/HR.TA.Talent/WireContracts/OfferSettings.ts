//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface OfferSettings {
    offerFeature?: OfferFeature[];
    offerExpiry?: OfferExpirySettings;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
    offerAcceptanceRedirectionSettings?: OfferAcceptanceRedirectionSettings;
}
