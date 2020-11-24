//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using MS.GTA.ServicePlatform.Privacy;

namespace MS.GTA.ServicePlatform.Exceptions.Cache
{
    internal sealed class CustomDataPropertyReference
    {
        private readonly Func<object, object> valueGetter;

        public CustomDataPropertyReference(string name, Func<object, object> valueGetter, PrivacyLevel privacyLevel, bool serializable)
        {
            Name = name;
            PrivacyLevel = privacyLevel;
            Serialize = serializable;

            this.valueGetter = valueGetter;
        }

        public string Name { get; }

        public PrivacyLevel PrivacyLevel { get; }

        public bool Serialize { get; }

        public object GetValue(Type type) => valueGetter(type);

        public CustomData Bind(object instance)
        {
            return new CustomData(
                Name,
                this.GetValue(instance)?.ToString(),
                PrivacyLevel,
                Serialize);
        }

        public object GetValue(object instance)
        {
            return this.valueGetter(instance);
        }
    }
}
