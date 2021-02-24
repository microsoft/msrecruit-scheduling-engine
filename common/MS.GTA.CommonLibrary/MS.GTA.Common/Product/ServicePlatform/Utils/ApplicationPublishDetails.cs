//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Product.ServicePlatform.Utils
{
    /// <summary>
    /// Details related to an application that can be published to external consumers.
    /// </summary>
    public class ApplicationPublishDetails
    {
        /// <summary>
        /// Gets or sets name of the application.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets description of the application.
        /// </summary>
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// Gets or sets the version of the application.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the email address that the consumers can reach out to for more details.
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the name of <see cref="Contact"/>.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the url of <see cref="Contact"/>.
        /// </summary>
        public string ContactUrl { get; set; }

        /// <summary>
        /// Gets or sets a boolean to indicate whether the application is secured by Bearer token authentication.
        /// </summary>
        public bool IsAuthorizedByBearer { get; set; }

        /// <summary>
        /// Gets or sets the route to the application documentation.
        /// </summary>
        public string DocumentationRoute { get; set; }
    }
}
