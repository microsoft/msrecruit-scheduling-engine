﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace HR.TA.ServicePlatform.Context
{
    using HR.TA.CommonDataService.Common.Internal;

    internal static class ActivityContextExtensions
    {
        public const string HttpStatusCodeProperty = "HttpStatusCode";

        public static void AddHttpStatusCode(this ServiceContext.Activity activity, int statusCode)
        {
            Contract.AssertValue(activity, nameof(activity));
            activity.AddCustomProperty(HttpStatusCodeProperty, statusCode.ToString());
        }
    }
}
