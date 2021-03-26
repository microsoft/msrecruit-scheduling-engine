//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Web.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using HR.TA.Common.Web.MiddleWare;    

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
