//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HeaderOperationFilter.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.ServicePlatform.AspNetCore.Builder.Filters
{
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-ms-session-id",
                In = "header",
                Type = "string",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-ms-root-activity-id",
                In = "header",
                Type = "string",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-ms-app-name",
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}
