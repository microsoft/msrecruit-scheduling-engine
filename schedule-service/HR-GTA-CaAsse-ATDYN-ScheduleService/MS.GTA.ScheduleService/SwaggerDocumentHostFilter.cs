//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MS.GTA.ScheduleService
{
    /// <summary>
    /// Implement IDocumentFilter and set the desired Host and BasePath values here.
    /// </summary>
    public class SwaggerDocumentHostFilter : IDocumentFilter
    {
        private readonly string hostName;
        private readonly string basePath;
        private readonly string scheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerDocumentHostFilter"/> class.
        /// </summary>
        /// <param name="configuration">Getting host and basepath value from appsettings.json</param>
        public SwaggerDocumentHostFilter(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Contract.CheckValue(configuration, nameof(configuration));
            this.hostName = configuration["SwaggerDocData:hostName"];
            this.basePath = configuration["SwaggerDocData:basePath"];
            this.scheme = configuration["SwaggerDocData:scheme"];
        }

        /// <summary>
        /// Implements Apply method of IDocumentFilter
        /// </summary>
        /// <param name="swaggerDoc">SwaggerDocument instance</param>
        /// <param name="context">DocumentFilterContext instance</param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Host = this.hostName;
            swaggerDoc.BasePath = this.basePath;
            swaggerDoc.Schemes = new List<string> { this.scheme };
        }
    }
}
