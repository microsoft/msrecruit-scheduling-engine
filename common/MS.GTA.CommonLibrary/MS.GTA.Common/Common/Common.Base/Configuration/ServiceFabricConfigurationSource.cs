// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ServiceFabricConfigurationSource.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Configuration
{
    using System.Fabric;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// ServiceFabricConfigurationSource class
    /// </summary>
    public class ServiceFabricConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// CodePackageActivationContext instance
        /// </summary>
        private readonly CodePackageActivationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFabricConfigurationSource" /> class.
        /// </summary>
        /// <param name="codePackageActivationContext">Instance of activation context for ServiceFabric service</param>
        public ServiceFabricConfigurationSource(CodePackageActivationContext codePackageActivationContext)
        {
            this.context = codePackageActivationContext;
        }

        /// <summary>
        /// Builds application configuration
        /// </summary>
        /// <param name="builder">Configuration builder instance</param>
        /// <returns>Configuration Provider</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(this.context);
        }
    }
}