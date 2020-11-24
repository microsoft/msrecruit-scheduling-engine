//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Instrumentation.Privacy;
using MS.GTA.ServicePlatform.Privacy;

namespace MS.GTA.ServicePlatform.Exceptions
{
    public sealed class CustomData : PrivateDataContainer<string>
    {
        private static IPrivacyMarker marker = new XmlPrivacyMarker(escapeContent: true);

        public CustomData(string name, string value, PrivacyLevel privacyLevel, bool serializable)
            : base(value, privacyLevel)
        {
            Name = name;
            Serialize = serializable;
        }

        public string Name { get; }

        public object MarkedValue
        {
            get
            {
                if (this.PrivacyLevel == PrivacyLevel.PublicData)
                {
                    return this.Value;
                }

                return marker.ToCompliantValue(this);
            }
        }

        public bool Serialize { get; }

        public string ToCompliantString() => MarkedValue.ToString();

        public string ToOriginalString() => Value;
    }
}
