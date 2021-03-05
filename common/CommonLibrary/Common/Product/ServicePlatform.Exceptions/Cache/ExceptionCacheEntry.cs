//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace ServicePlatform.Exceptions.Cache
{
    internal class ExceptionCacheEntry
    {
        public ExceptionCacheEntry(string typeName, MonitoredExceptionMetadataAttribute metadata, IEnumerable<CustomDataPropertyReference> customDataReferences)
        {
            this.TypeName = typeName;
            this.Metadata = metadata;
            this.CustomDataPropertyReferences = customDataReferences;
        }

        public string TypeName { get; private set; }

        public MonitoredExceptionMetadataAttribute Metadata { get; private set; }

        public IEnumerable<CustomDataPropertyReference> CustomDataPropertyReferences { get; private set; }
    }
}
