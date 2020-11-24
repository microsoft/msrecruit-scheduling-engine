using MS.GTA.Common.TalentAttract.Contract;
using MS.GTA.Common.XrmHttp;
using MS.GTA.Common.XrmHttp.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MS.GTA.XrmData.Query.Attract.OptionSetMetadata
{
    public class OptionSetInfo
    {
        private Dictionary<string, IList<PicklistAttributeMetadata>> metadata = new Dictionary<string, IList<PicklistAttributeMetadata>>(StringComparer.OrdinalIgnoreCase);

        private CultureInfo cultureInfo;

        public OptionSetInfo(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public void AddOptionSetInfo(string logicalName, IList<PicklistAttributeMetadata> picklistAttributeMetadata)
        {
            this.metadata.Add(logicalName, picklistAttributeMetadata);
        }

        public string GetOptionValueLabel<T>(Expression<Func<T, object>> field, int value, CultureInfo cultureInfo = null)
        {
            return this.GetOptionValueLabel(
                ODataEntityContractInfo.GetEntitySingularName(typeof(T)),
                ODataField.Field<T>(field).ToString(),
                value,
                cultureInfo);
        }

        public string GetOptionValueLabel(string entity, string field, int value, CultureInfo cultureInfo = null)
        {
            if (this.metadata.TryGetValue(entity, out var picklistAttributeMetadata))
            {
                var attributeMetadata = picklistAttributeMetadata
                    .FirstOrDefault(a => a.LogicalName.Equals(field, StringComparison.OrdinalIgnoreCase));
                if (attributeMetadata != null)
                {
                    return attributeMetadata.OptionSet.GetOptionValueLabelForCulture(value, cultureInfo ?? this.cultureInfo);
                }
            }

            return null;
        }

        public OptionSetValue GetOptionSetValue<T>(T entity, Expression<Func<T, object>> field)
        {
            if (entity == null)
            {
                return null;
            }

            var entityName = ODataEntityContractInfo.GetEntitySingularName(typeof(T));
            var fieldName = ODataField.Field(field).ToString();

            var fieldFunc = field.Compile();
            var value = fieldFunc(entity);
            if (value == null)
            {
                return new OptionSetValue
                {
                    OptionSet = $"{entityName}.{fieldName}",
                };
            }

            var valueInt = Convert.ToInt32(value);

            if (this.metadata.TryGetValue(entityName, out var picklistAttributeMetadata))
            {
                var attributeMetadata = picklistAttributeMetadata
                    .FirstOrDefault(a => a.LogicalName.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
                if (attributeMetadata != null)
                {
                    return new OptionSetValue
                    {
                        Value = valueInt,
                        Label = attributeMetadata.OptionSet.GetOptionValueLabelForCulture(valueInt, this.cultureInfo),
                        OptionSet = $"{entityName}.{fieldName}",
                    };
                }
            }

            return null;
        }
    }
}
