//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Web.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using CommonLibrary.Common.Web.MiddleWare;    

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
