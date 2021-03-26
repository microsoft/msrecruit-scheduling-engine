//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Web.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using TA.CommonLibrary.Common.Web.MiddleWare;    

    public static class ExceptionMiddlewareExtensions
    {
        public static IMvcBuilder AddMvcWithCustomExceptionHandler(this IServiceCollection services)
        {
            return services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(CustomExceptionMiddleware));
                });
        }
    }
}
