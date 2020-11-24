//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AdditionalMetadataExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.TalentAttract.Contract;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public static class AdditionalMetadataExtensions
    {
        public static Dictionary<string, AdditionalMetadataValue> DeserializeAdditionalMetadata(string additionalMetadata)
        {
            try
            {
                return string.IsNullOrEmpty(additionalMetadata)
                    ? null
                    : JsonConvert.DeserializeObject<Dictionary<string, AdditionalMetadataValue>>(additionalMetadata);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Dictionary<string, string> DeserializeStringOnlyAdditionalMetadata(string additionalMetadata)
        {
            return DeserializeAdditionalMetadata(additionalMetadata)
                ?.Where(kv => kv.Value.Type == AdditionalMetadataValueType.String)
                ?.ToDictionary(k => k.Key, k => k.Value.Value);
        }
    }
}
