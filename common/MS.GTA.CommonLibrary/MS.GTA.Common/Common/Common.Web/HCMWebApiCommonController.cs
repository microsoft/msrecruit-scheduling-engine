//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HCMWebApiCommonController.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Base.Security;
    using MS.GTA.ServicePlatform.Context;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// HCMWebAPICommon Controller class
    /// </summary>
    public class HCMWebApiCommonController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HCMWebApiCommonController" /> class.
        /// </summary>
        /// <param name="contextAccessor">HTTP Context Accessor</param>
        public HCMWebApiCommonController(IHttpContextAccessor contextAccessor)
        {
            var hcmAppPrincipal = new HCMApplicationPrincipal(contextAccessor.HttpContext);
            ServiceContext.Principal.SetPrincipal(hcmAppPrincipal);
        }
    }
}