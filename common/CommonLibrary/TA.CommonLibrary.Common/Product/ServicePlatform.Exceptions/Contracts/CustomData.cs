//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.CommonDataService.Instrumentation.Privacy;
using TA.CommonLibrary.ServicePlatform.Privacy;

namespace TA.CommonLibrary.ServicePlatform.Exceptions
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
