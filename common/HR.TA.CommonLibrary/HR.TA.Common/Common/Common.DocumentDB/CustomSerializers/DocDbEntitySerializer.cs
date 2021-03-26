//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.DocumentDB.CustomSerializers
{
    using System;
    using Contracts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>The doc DB entity serializer.</summary>
    public class DocDbEntitySerializer : JsonConverter
    {
        /// <summary>The OID.</summary>
        private readonly string oid;

        /// <summary>Initializes a new instance of the <see cref="DocDbEntitySerializer"/> class.</summary>
        /// <param name="oid">The OID of the user that the serializer will use when creating or updating the document.</param>
        public DocDbEntitySerializer(string oid = null)
        {
            this.oid = oid;
        }

        /// <summary>The write JSON.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var docDbEntity = (DocDbEntity)value;
            var json = JObject.FromObject(docDbEntity);

            json.Add("Type", docDbEntity.GetType().Name);

            if (docDbEntity.CreatedBy != null)
            {
                json.Add(nameof(DocDbEntity.CreatedBy), docDbEntity.CreatedBy);
            } 
            else if (this.oid != null)
            {
                json.Add(nameof(DocDbEntity.CreatedBy), this.oid);
            }

            if (docDbEntity.UpdatedBy != null)
            {
                json.Add(nameof(DocDbEntity.UpdatedBy), docDbEntity.UpdatedBy);
            }
            else if (this.oid != null)
            {
                json.Add(nameof(DocDbEntity.UpdatedBy), this.oid);
            }

            if (docDbEntity.CreatedAt != default(long))
            {
                json.Add(nameof(DocDbEntity.CreatedAt), docDbEntity.CreatedAt);
                json.Add(nameof(DocDbEntity.UpdatedAt), DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
            else
            {
                json.Add(nameof(DocDbEntity.CreatedAt), DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }

            json.WriteTo(writer);
        }

        /// <summary>The read JSON.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var docDbEntity = JObject.Load(reader);
            var converted = docDbEntity.ToObject(objectType) as DocDbEntity;
            if (converted == null)
            {
                return null;
            }

            if (docDbEntity[nameof(DocDbEntity.CreatedAt)] != null)
            {
                converted.CreatedAt = docDbEntity[nameof(DocDbEntity.CreatedAt)].ToObject<long>();
                converted.CreatedDateTime = DateTimeOffset.FromUnixTimeSeconds(converted.CreatedAt).UtcDateTime;
            }

            if (docDbEntity[nameof(DocDbEntity.UpdatedAt)] != null)
            {
                converted.UpdatedAt = docDbEntity[nameof(DocDbEntity.UpdatedAt)].ToObject<long>();
                converted.UpdatedDateTime = DateTimeOffset.FromUnixTimeSeconds(converted.UpdatedAt).UtcDateTime;
            }

            if (docDbEntity[nameof(DocDbEntity.CreatedBy)] != null)
            {
                converted.CreatedBy = docDbEntity[nameof(DocDbEntity.CreatedBy)].ToObject<string>();
            }

            if (docDbEntity[nameof(DocDbEntity.UpdatedBy)] != null)
            {
                converted.UpdatedBy = docDbEntity[nameof(DocDbEntity.UpdatedBy)].ToObject<string>();
            }

            return converted;
        }

        /// <summary>The can convert.</summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(DocDbEntity));
    }
}
