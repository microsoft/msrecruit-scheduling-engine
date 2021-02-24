//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Xml;
    using System.Runtime.Serialization;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Class to resolve known organization request/response types for the SDK contracts. Wrapped with a lock in order to be thread-safe.
    /// </summary>
    internal sealed class LockedKnownTypesResolver : DataContractResolver
    {
        KnownTypesResolver resolver;
        object syncObject;

        public LockedKnownTypesResolver()
        {
            this.syncObject = new object();
            this.resolver = new KnownTypesResolver();
        }

        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            lock (this.syncObject)
            {
                return this.resolver.ResolveName(typeName, typeNamespace, declaredType, knownTypeResolver);
            }
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            lock (this.syncObject)
            {
                return this.resolver.TryResolveType(type, declaredType, knownTypeResolver, out typeName, out typeNamespace);
            }
        }
    }
}
