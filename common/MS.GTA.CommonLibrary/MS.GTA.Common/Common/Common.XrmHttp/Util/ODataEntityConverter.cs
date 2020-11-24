//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class ODataEntityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ODataEntity).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var contract = serializer.ContractResolver.ResolveContract(objectType) as JsonObjectContract;
                var value = (ODataEntity)contract.DefaultCreator();

                foreach (var property in JObject.Load(reader).Properties())
                {
                    if (property.Value.Type == JTokenType.Null)
                    {
                        continue;
                    }

                    var propertyName = property.Name;
                    if (propertyName.EndsWith("@odata.bind"))
                    {
                        // "child@odata.bind": "/childentity(GUID)"
                        // -> new Entity { KeyField = GUID }
                        propertyName = propertyName.Substring(0, propertyName.Length - 11);

                        var jsonProperty = contract.Properties.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
                        if (jsonProperty != null)
                        {
                            var propertyType = jsonProperty.PropertyType;

                            var jsonPropertyContract = serializer.ContractResolver.ResolveContract(propertyType) as JsonObjectContract;
                            var keyProperty = jsonPropertyContract.Properties.FirstOrDefault(p => p.AttributeProvider.GetAttributes(typeof(KeyAttribute), false).Any());

                            var propertyValue = jsonPropertyContract.DefaultCreator();
                            keyProperty.ValueProvider.SetValue(propertyValue, ODataUriParse.GetEntityIdFromUri(property.Value.ToString()));
                            jsonProperty.ValueProvider.SetValue(value, propertyValue);
                        }
                    }
                    else
                    {
                        // Default case
                        var jsonProperty = contract.Properties.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
                        if (jsonProperty != null)
                        {
                            var propertyType = jsonProperty.PropertyType;
                            jsonProperty.ValueProvider.SetValue(value, property.Value.ToObject(propertyType, serializer));
                        }
                        else
                        {
                            contract.ExtensionDataSetter?.Invoke(value, propertyName, property.Value);
                        }
                    }
                }

                return value;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var contract = serializer.ContractResolver.ResolveContract(value.GetType()) as JsonObjectContract;

            writer.WriteStartObject();

            foreach (var property in contract.Properties)
            {
                if (property.Ignored || !property.Readable)
                {
                    continue;
                }

                if (property.PropertyName.StartsWith("_") && property.PropertyName.EndsWith("_value"))
                {
                    // Skip properties that map to entity reference properties in xrm. These fields cannot be modified directly.
                    // The property that maps to the navigation property in xrm must be used instead.
                    continue;
                }

                var propertyValue = property.ValueProvider.GetValue(value);
                var propertyType = property.PropertyType;

                var defaultValueHandling = property.DefaultValueHandling ?? serializer.DefaultValueHandling;
                var defaultValue = property.DefaultValue ?? (propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null);
                if (defaultValueHandling.HasFlag(DefaultValueHandling.Ignore)
                    && object.Equals(propertyValue, defaultValue))
                {
                    // Skip default(T)
                    continue;
                }

                WriteProperty(writer, serializer, property.PropertyName, propertyValue, propertyType);
            }

            var extensionData = contract.ExtensionDataGetter?.Invoke(value);
            if (extensionData != null)
            {
                foreach (var pair in extensionData)
                {
                    if (pair.Key != null && pair.Value != null)
                    {
                        WriteProperty(writer, serializer, pair.Key as string, pair.Value, pair.Value.GetType());
                    }
                }
            }

            writer.WriteEndObject();
        }

        private static void WriteProperty(JsonWriter writer, JsonSerializer serializer, string propertyName, object propertyValue, Type propertyType)
        {
            if (typeof(ODataEntity).IsAssignableFrom(propertyType) && propertyValue != null)
            {
                var propertyContract = serializer.ContractResolver.ResolveContract(propertyType) as JsonObjectContract;

                var contentId = GetPropertyByName(propertyContract, propertyValue, nameof(ODataEntity.ODataBatchContentIdReference));
                if (contentId != null)
                {
                    // Field.ODataBatchContentIdReference -> "field@odata.bind": "$contentId"
                    writer.WritePropertyName(propertyName + "@odata.bind");
                    writer.WriteValue($"${contentId}");
                    return;
                }

                var propertyKeyProperty = propertyContract.Properties.FirstOrDefault(p => p.AttributeProvider.GetAttributes(typeof(KeyAttribute), false).Any());
                var propertyKeyPropertyValue = propertyKeyProperty?.ValueProvider.GetValue(propertyValue);
                var propertyKeyPropertyType = propertyKeyProperty.PropertyType;

                var propertyKeyPropertyDefaultValue = propertyKeyProperty.DefaultValue ?? (propertyKeyPropertyType.IsValueType ? Activator.CreateInstance(propertyKeyPropertyType) : null);
                if (!object.Equals(propertyKeyPropertyValue, propertyKeyPropertyDefaultValue))
                {
                    // Field.Key -> "field@odata.bind": "/Entity(id)"
                    var entityName = ODataEntityContractInfo.GetEntityPluralName(propertyType);
                    writer.WritePropertyName(propertyName + "@odata.bind");
                    writer.WriteValue($"/{entityName}({propertyKeyPropertyValue})");
                    return;
                }
            }

            // Default behavior
            writer.WritePropertyName(propertyName);
            serializer.Serialize(writer, propertyValue);
        }

        private static object GetPropertyByName(JsonObjectContract objContract, object obj, string propName)
        {
            return objContract.Properties.GetProperty(propName, StringComparison.Ordinal).ValueProvider.GetValue(obj);
        }
    }
}
