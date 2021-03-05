//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Address {
    AddressLine1?: string;
    AddressLine2?: string;
    City?: string;
    State?: string;
    Country?: CountryCode;
    PostalCode?: string;
}
