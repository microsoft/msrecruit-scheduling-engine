//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace MS.GTA.Common.XrmHttp.Util
{
    public static class TalentMetadataExtensions
    {
        public static (string entityName, string customFieldName) GetEntityAndCustomField(string propertyName)
        {
            var indexOfPipe = propertyName.IndexOf('|');
            var lengthPrefix = propertyName.Substring(0, indexOfPipe);
            var suffix = propertyName.Substring(indexOfPipe + 1);
            var lengthsArray = lengthPrefix.Split(';');

            int.TryParse(lengthsArray[0], out var entityNameLength);
            int.TryParse(lengthsArray[1], out var customFieldLength);

            var entityName = suffix.Substring(0, entityNameLength);
            var customFieldName = suffix.Substring(entityNameLength, customFieldLength);

            return (entityName, customFieldName);
        }

        public static string GetCustomPropertyName(string entityLogicalName, string customFieldName)
        {
            var concatName =
                $"{entityLogicalName.Length};{customFieldName.Length}|{entityLogicalName}{customFieldName}";
            return concatName;
        }
    }
}
