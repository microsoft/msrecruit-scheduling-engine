//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SafeEnumConverter.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Serialization
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Creates an enum from its string value, invalid values are read as zero. Any value which cannot be cast to the enum (values which are not
    /// strings or integers, or strings which are not enum values) will be read as a zero and cast to the enum type. This is typically the first enum value.
    /// </summary>
    public class SafeEnumConverter : StringEnumConverter
    {
        /// <summary> The default enum value</summary>
        private const string defaultEnumName = "NotSpecified";

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        public override bool CanConvert(Type objectType)
        {
            return base.CanConvert(objectType) && objectType.IsValueType;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException ex)
            {
                var names = Enum.GetNames(objectType);
                var defaultEnumValueName = names.FirstOrDefault(name => string.Equals(name, defaultEnumName, StringComparison.OrdinalIgnoreCase));

                if (defaultEnumValueName == null)
                {
                    // Safety catch: Fall back to the first enum value
                    defaultEnumValueName = names.First();
                }

                return Enum.Parse(objectType, defaultEnumValueName);
            }
        }
    }
}