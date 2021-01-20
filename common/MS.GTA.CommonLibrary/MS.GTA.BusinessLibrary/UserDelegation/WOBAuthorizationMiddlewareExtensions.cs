//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WOBAuthorizationMiddlewareExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.BusinessLibrary.UserDelegation
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
