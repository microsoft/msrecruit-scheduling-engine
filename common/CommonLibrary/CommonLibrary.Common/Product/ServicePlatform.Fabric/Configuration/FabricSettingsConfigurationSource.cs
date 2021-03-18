//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Fabric;
using CommonLibrary.CommonDataService.Common.Internal;
using Microsoft.Extensions.Configuration;

namespace CommonLibrary.ServicePlatform.Fabric.Configuration
{
    public sealed class FabricSettingsConfigurationSource : IConfigurationSource
    {
        private readonly ICodePackageActivationContext fabricActivationContext;

        public FabricSettingsConfigurationSource()
        {
            fabricActivationContext = FabricRuntime.GetActivationContext();
        }

        public FabricSettingsConfigurationSource(ICodePackageActivationContext fabricActivationContext)
        {
            Contract.CheckValue(fabricActivationContext, nameof(fabricActivationContext));

            this.fabricActivationContext = fabricActivationContext;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            ServicePlatformFabricTrace.Instance.TraceInformation("Creating configuration provider from service fabric configuration package.");
            return new FabricSettingsConfigurationProvider(fabricActivationContext.GetConfigurationPackageObject("Config").Settings);
        }
    }
}
