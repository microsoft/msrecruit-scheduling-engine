//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonLibrary.ServicePlatform.Exceptions.Cache
{
    /// <summary>
    /// Provides caching functionality for exception reflection.
    /// </summary>
    internal static class ExceptionTypeCache
    {
        // void duplicating instances of unknown metadata
        private static readonly UnknownExceptionMetadataAttribute UnkownMetadata = new UnknownExceptionMetadataAttribute();
        private static readonly ConcurrentDictionary<Type, ExceptionCacheEntry> reflectionCache = new ConcurrentDictionary<Type, ExceptionCacheEntry>();

        public static ExceptionCacheEntry GetCacheEntry(Type type)
        {
            return reflectionCache.GetOrAdd(type, BuildCacheEntry);
        }

        private static ExceptionCacheEntry BuildCacheEntry(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();

            var exceptionMetadata = typeInfo.GetCustomAttribute<MonitoredExceptionMetadataAttribute>();

            if (exceptionMetadata == null)
            {
                exceptionMetadata = UnkownMetadata;
            }

            var customDataPropertyReferences = new List<CustomDataPropertyReference>();
                
            foreach (var prop in GetAllDeclaredProperties(typeInfo))
            {
                var attr = prop.GetCustomAttribute<ExceptionCustomDataAttribute>();
                if (attr != null)
                {
                    var accessor = prop.GetMethod;
                    customDataPropertyReferences.Add(new CustomDataPropertyReference(
                        attr.Name ?? prop.Name,
                        o => accessor.Invoke(o, null),
                        attr.PrivacyLevel,
                        attr.Serialize));
                }
            }

            return new ExceptionCacheEntry(type.FullName, exceptionMetadata, customDataPropertyReferences);
        }

        private static IEnumerable<PropertyInfo> GetAllDeclaredProperties(TypeInfo typeInfo)
        {
            while (typeInfo != null)
            {
                foreach (var prop in typeInfo.DeclaredProperties)
                {
                    yield return prop;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}
