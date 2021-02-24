//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Util
{
    using System.Collections.Generic;
    using Contracts;
    using Newtonsoft.Json.Linq;

    public static class CustomFieldExtensions
    {
        public static Dictionary<string, JToken> GetCustomFields(this ODataEntity entity, Dictionary<string, JToken> existingDictionary = null)
        {
            var propertyBag = existingDictionary ?? new Dictionary<string, JToken>();

            if (entity?.ODataUnmappedFields == null)
            {
                return propertyBag;
            }

            var entityName = entity.GetSingularName();
            foreach (var unmappedField in entity.ODataUnmappedFields)
            {
                var customFieldName = unmappedField.Key;
                var anchorFieldName = TalentMetadataExtensions.GetCustomPropertyName(entityName, customFieldName);
                propertyBag[anchorFieldName] = unmappedField.Value;
            }

            return propertyBag;
        }


        public static Dictionary<string, JToken> GetCutomFields(this TalentBaseContract extendedContract, Dictionary<string, JToken> existingDictionary = null)
        {
            var dictionary = existingDictionary ?? new Dictionary<string, JToken>();
            if (extendedContract?.CustomFields == null)
            {
                return dictionary;
            }

            foreach (var extendedProperty in extendedContract.CustomFields)
            {
                var anchorFieldName = extendedProperty.Key;
                var (entityName, customFieldName) = TalentMetadataExtensions.GetEntityAndCustomField(anchorFieldName);
                dictionary[customFieldName] = extendedProperty.Value;
            }

            return dictionary;
        }
    }
}
