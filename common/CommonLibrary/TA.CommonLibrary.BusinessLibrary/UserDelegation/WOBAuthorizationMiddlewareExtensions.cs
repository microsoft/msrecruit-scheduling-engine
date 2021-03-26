//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace TA.CommonLibrary.BusinessLibrary.UserDelegation
{
    using Microsoft.AspNetCore.Builder;
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// Middleware extension used for WOB Auth.
    /// </summary>
    public static class WOBAuthorizationMiddlewareExtensions
    {
        /// <summary>
        /// This method to be used in startup for the middelware.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWobAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WOBAuthorization>();
        }
    }
}
