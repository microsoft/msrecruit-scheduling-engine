//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class ODataEntityContractInfo
    {
        /// <summary>
        /// Gets the entity plural name from the DataContract attribute on the contract class.
        /// </summary>
        /// <param name="type">The contract class type.</param>
        /// <returns>The entity plural name.</returns>
        public static string GetEntityPluralName(Type type)
        {
            return type.GetCustomAttributes(typeof(ODataEntityAttribute), false).Cast<ODataEntityAttribute>().FirstOrDefault()?.PluralName
                ?? type.GetCustomAttributes(typeof(DataContractAttribute), false).Cast<DataContractAttribute>().FirstOrDefault()?.Name
                ?? type.Name;
        }

        public static string GetPluralName(this ODataEntity type)
        {
            return GetEntityPluralName(type.GetType());
        }

        public static string GetSingularName(this ODataEntity type)
        {
            return GetEntitySingularName(type.GetType());
        }

        public static string GetEntitySingularName(Type type)
        {
            return type.GetCustomAttributes(typeof(ODataEntityAttribute), false).Cast<ODataEntityAttribute>().FirstOrDefault()?.SingularName
                ?? type.GetCustomAttributes(typeof(DataContractAttribute), false).Cast<DataContractAttribute>().FirstOrDefault()?.Name
                ?? type.Name;
        }

        public static string GetEntitySingularNameWithNamespace(Type type)
        {
            var name = GetEntitySingularName(type);

            var namesp = type.GetCustomAttributes(typeof(ODataNamespaceAttribute), true).Cast<ODataNamespaceAttribute>().FirstOrDefault()?.Namespace;
            if (string.IsNullOrEmpty(namesp))
            {
                return name;
            }

            return $"{namesp}.{name}";
        }

        public static PropertyInfo GetKeyProperty(Type t)
        {
            return t.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
        }

        public static Guid? GetKeyPropertyValue<T>(T entity)
        {
            return GetKeyProperty(typeof(T))?.GetValue(entity) as Guid?;
        }

        /// <summary>
        /// Gets the property name from the DataMember attribute on the contract class property.
        /// </summary>
        /// <param name="memberInfo">The property member information.</param>
        /// <returns>The property name.</returns>
        public static string GetPropertyNameFromMemberInfo(MemberInfo memberInfo)
        {
            var dataMemberAttributes = memberInfo.GetCustomAttributes(typeof(DataMemberAttribute), false);
            return dataMemberAttributes.Length == 0 ? memberInfo.Name : ((DataMemberAttribute)dataMemberAttributes[0]).Name;
        }
    }
}
