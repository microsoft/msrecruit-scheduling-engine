//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base.Configuration
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// FabricXmlConfigurationSource class
    /// </summary>
    public class FabricXmlConfigurationSource : FileConfigurationSource
    {
        /// <summary>
        /// Override Build method that constructs a configuration provider
        /// </summary>
        /// <param name="builder">Configuration builder object</param>
        /// <returns>Configuration Provider</returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new FabricXmlConfigurationProvider(this);
        }
    }
}
