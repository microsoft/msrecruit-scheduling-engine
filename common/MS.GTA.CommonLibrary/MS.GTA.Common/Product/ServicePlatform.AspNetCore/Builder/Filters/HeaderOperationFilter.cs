//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
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
                Description = "Session Id to track the client activity in a given session",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-ms-root-activity-id",
                In = "header",
                Type = "string",
                Description = "Unique identifier for the operation",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-ms-app-name",
                In = "header",
                Type = "string",
                Description = "Indicates the app name",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "X-CorrelationId",
                In = "header",
                Type = "string",
                Description = "Correlation Id for telemetry",
                Required = false
            });
        }
    }
}
