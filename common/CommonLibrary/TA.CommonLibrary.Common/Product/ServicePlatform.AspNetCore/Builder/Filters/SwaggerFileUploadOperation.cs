//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace TA.CommonLibrary.Common.Product.ServicePlatform.AspNetCore.Builder.Filters
{
    /// <summary>
    /// Swagger File Upload Operation
    /// </summary>
    public class SwaggerFileUploadOperation : IOperationFilter
    {
        private const string formDataMimeType = "multipart/form-data";

        /// <summary>
        /// Interface method to modify the documentation
        /// </summary>
        /// <param name="operation">Operation</param>
        /// <param name="context">Context</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Consumes != null && operation.Consumes.Count > 0)
            {
                var isFileUpload = operation.Consumes.FirstOrDefault(s => s.Equals(formDataMimeType));
                if (!string.IsNullOrEmpty(isFileUpload))
                {
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "files",
                        In = "formData",
                        Description = "Upload File",
                        Required = true,
                        Type = "file"
                    });
                }
            }
        }
    }
}
