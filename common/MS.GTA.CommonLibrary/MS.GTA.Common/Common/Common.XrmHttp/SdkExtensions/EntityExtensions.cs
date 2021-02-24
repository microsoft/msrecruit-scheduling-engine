//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public static class EntityExtensions
    {
        public static Entity GetTargetEntity(this RemoteExecutionContext context)
        {
            return context.InputParameters.TryGetValue("Target", out var target) ? target as Entity : null;
        }

        public static Entity GetPreImageEntity(this RemoteExecutionContext context)
        {
            return context.PreEntityImages.FirstOrDefault().Value;
        }

        public static T GetTarget<T>(this RemoteExecutionContext context)
            where T : class
        {
            return context.GetTargetEntity()?.ToContractClass<T>();
        }

        public static T GetPreImage<T>(this RemoteExecutionContext context)
            where T : class
        {
            return context.GetPreImageEntity()?.ToContractClass<T>();
        }

        public static T GetPreImage<T>(this RemoteExecutionContext context, string name)
            where T : class
        {
            return context.PreEntityImages.FirstOrDefault(i => i.Key == name).Value?.ToContractClass<T>();
        }

        public static T GetPostImage<T>(this RemoteExecutionContext context)
            where T : class
        {
            return context.PostEntityImages.FirstOrDefault().Value?.ToContractClass<T>();
        }

        public static T GetPostImage<T>(this RemoteExecutionContext context, string name)
            where T : class
        {
            return context.PostEntityImages.FirstOrDefault(i => i.Key == name).Value?.ToContractClass<T>();
        }

        public static bool IsFieldChanged<T>(this RemoteExecutionContext context, Expression<Func<T, object>> field)
            where T : class
        {
            // Get the update info from the context.
            var target = context.GetTargetEntity();
            if (target == null)
            {
                // No update info means no field changed.
                return false;
            }

            // Find the attribute in the update info.
            var fieldName = ODataField.Field(field).ToFetchXmlFieldName();
            var targetAttribute = target.Attributes.FirstOrDefault(a => fieldName.Equals(a.Key, StringComparison.OrdinalIgnoreCase)).Value;
            if (targetAttribute == null)
            {
                // Attribute not in the update info means the field didn't change.
                return false;
            }

            // Get the old values to see if it was a no-op update.
            var preImage = context.GetPreImage<T>();
            if (preImage == null)
            {
                // Old values not available, so assume it was not a no-op.
                return true;
            }

            // Convert the update info to the contract class.
            var targetObject = target.ToContractClass<T>();

            // Compare the old and new values - if they are different, then the field was updated.
            var fieldFunc = field.Compile();
            return !Equals(fieldFunc(targetObject), fieldFunc(preImage));
        }

        public static T ToContractClass<T>(this Entity entity)
        {
            var jsonSerializer = JsonSerializer.Create(XrmHttpClient.DefaultJsonSerializerSettings);
            var contract = jsonSerializer.ContractResolver.ResolveContract(typeof(T)) as JsonObjectContract;

            if (entity.Attributes == null)
            {
                return default(T);
            }

            return new JObject(entity.Attributes
                .Select(p =>
                {
                    var key = p.Key;
                    var value = p.Value;

                    if (value is AliasedValue aliasedValue)
                    {
                        value = aliasedValue.Value;
                    }

                    if (value is string stringValue
                        && stringValue.StartsWith("/Date(")
                        && stringValue.EndsWith(")/"))
                    {
                        value = JsonConvert.DeserializeObject<DateTimeOffset>($@"""{stringValue}""");
                    }

                    if (value is OptionSetValue optionSetValue)
                    {
                        value = optionSetValue.Value;
                    }
                    else if (value is OptionSetValueCollection optionSetValueCollection)
                    {
                        value = string.Join(",", optionSetValueCollection.Select(v => v.Value));
                    }
                    else if (value is BooleanManagedProperty boolean)
                    {
                        value = boolean.Value;
                    }
                    else if (value is Money moneyValue)
                    {
                        value = moneyValue.Value;
                    }
                    else if (value is EntityReference entityReference)
                    {
                        var contractProperty = contract.Properties.GetClosestMatchProperty($"_{key}_value");
                        key = contractProperty?.PropertyName ?? key;
                        value = entityReference.Id;
                    }

                    try
                    {
                        return new JProperty(key, value);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(p => p != null)
                .ToArray()).ToObject<T>(jsonSerializer);
        }

        public static T CopyAttributesOnto<T>(this Entity entity, T target)
        {
            var jsonSerializer = JsonSerializer.Create(XrmHttpClient.DefaultJsonSerializerSettings);
            var contract = jsonSerializer.ContractResolver.ResolveContract(typeof(T)) as JsonObjectContract;

            if (entity?.Attributes == null)
            {
                return target;
            }

            foreach (var attribute in entity.Attributes)
            {
                var key = attribute.Key;
                var value = attribute.Value;
                var contractProperty = contract.Properties.GetClosestMatchProperty(key);

                if (value is AliasedValue aliasedValue)
                {
                    value = aliasedValue.Value;
                }

                if (value is string stringValue
                    && stringValue.StartsWith("/Date(")
                    && stringValue.EndsWith(")/"))
                {
                    value = JsonConvert.DeserializeObject<DateTimeOffset>($@"""{stringValue}""");
                }

                if (value is OptionSetValue optionSetValue)
                {
                    value = optionSetValue.Value;
                }
                else if (value is OptionSetValueCollection optionSetValueCollection)
                {
                    value = string.Join(",", optionSetValueCollection.Select(v => v.Value));
                }
                else if (value is BooleanManagedProperty boolean)
                {
                    value = boolean.Value;
                }
                else if (value is Money moneyValue)
                {
                    value = moneyValue.Value;
                }
                else if (value is EntityReference entityReference)
                {
                    contractProperty = contract.Properties.GetClosestMatchProperty($"_{key}_value") ?? contractProperty;
                    value = entityReference.Id;
                }

                try
                {
                    if (contractProperty != null)
                    {
                        var valueObject = new JValue(value).ToObject(contractProperty.PropertyType);
                        contractProperty.ValueProvider.SetValue(target, valueObject);
                    }
                }
                catch
                {
                    // Do nothing.
                }
            }

            return target;
        }
    }
}
