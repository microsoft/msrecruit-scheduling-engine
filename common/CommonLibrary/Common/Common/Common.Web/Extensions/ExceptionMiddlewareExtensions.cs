//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Web.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Common.Web.MiddleWare;    

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
