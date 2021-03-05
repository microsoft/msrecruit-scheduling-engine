//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ExpressionSuggestionCollection {
    id: string;
    name?: string;
    reason?: string;
    suggestion?: string;
    languageCode?: string;
    isEnabled?: boolean;
    hasDefault?: boolean;
    expressionSuggestions?: ExpressionSuggestion[];
}
