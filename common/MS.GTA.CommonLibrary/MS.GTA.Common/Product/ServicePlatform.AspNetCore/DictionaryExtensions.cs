﻿//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Collections.Generic;
using MS.GTA.CommonDataService.Common.Internal;
using Microsoft.Extensions.Primitives;

namespace MS.GTA.ServicePlatform.AspNetCore
{
    internal static class DisctionaryExtensions
    {
        internal static string TryGetFirstOrDefaultValue(this IDictionary<string, StringValues> headers, string headerName)
        {
            Contract.AssertValue(headers, nameof(headers));
            Contract.AssertValue(headerName, nameof(headerName));

            string headerValue = string.Empty;

            StringValues headerValues;
            if (headers.TryGetValue(headerName, out headerValues) && headerValues.Count > 0)
            {
                headerValue = headerValues[0];
            }

            return headerValue;
        }
    }
}
