//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ExternalJobPost extends TalentBaseContract {
    id?: string;
    uri?: string;
    supplier?: JobPostSupplier;
    supplierName?: string;
    isRepostAvailable?: boolean;
    userAction?: UserAction;
    extendedAttributes?: { [key: string]: string; };
}
