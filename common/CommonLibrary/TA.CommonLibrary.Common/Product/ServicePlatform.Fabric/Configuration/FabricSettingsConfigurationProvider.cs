//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Fabric.Description;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Context;
using TA.CommonLibrary.ServicePlatform.Tracing;
using Microsoft.Extensions.Configuration;

namespace TA.CommonLibrary.ServicePlatform.Fabric.Configuration
{
    internal sealed class FabricSettingsConfigurationProvider : ConfigurationProvider
    {
        private readonly ConfigurationSettings fabricConfigurationSettings;

        internal FabricSettingsConfigurationProvider(ConfigurationSettings fabricConfigurationSettings)
        {
            Contract.AssertValue(fabricConfigurationSettings, nameof(fabricConfigurationSettings));

            this.fabricConfigurationSettings = fabricConfigurationSettings;
        }

        private ITraceSource Tracer
        {
            get { return ServicePlatformFabricTrace.Instance; }
        }

        // TODO
        /*
        public override void Load()
        {
            ServiceContext.Activity.Execute(
                FabricSettingsLoadActivity.Instance,
                () =>
                {
                    var sections = this.fabricConfigurationSettings.Sections;

                    var settings = new Dictionary<string, string>();
                    foreach (var configurationSection in sections)
                    {
                        var sectionName = configurationSection.Name;
                        foreach (var parameter in configurationSection.Parameters)
                        {
                            var settingKey = $"{sectionName}:{parameter.Name}";
                            var settingValue = parameter.Value;
                            
                            // HACK: This is temporary support for Encrypted Parameters.
                            //       This will be removed once SecreteManager work is complete.
                            //       Do not take a dependency here.
                            if (parameter.IsEncrypted)
                            {
                                // Work-around to extract cleartext of a SecureString
                                // http://stackoverflow.com/questions/818704/how-to-convert-securestring-to-system-string
                                settingValue = new System.Net.NetworkCredential(string.Empty, parameter.DecryptValue()).Password;
                            }

                            settings.Add(settingKey, settingValue);

                            var settingValueForTrace = parameter.IsEncrypted
                                ? "[ENCRYPTED]"
                                : settingValue;

                            Tracer.TraceInformation("Add setting: \"{0}\" : \"{1}\"", settingKey, settingValueForTrace);
                        }
                    }

                    Data = settings;
                });
        }*/
    }
}
