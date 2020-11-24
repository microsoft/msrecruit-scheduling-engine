using MS.GTA.ServicePlatform.Exceptions;
using MS.GTA.ServicePlatform.Privacy;

namespace MS.GTA.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// An implementation of <see cref="IPrivacyMarker"/> that marks sensitive data using XML tags.
    /// </summary>
    public class XmlPrivacyMarker : IPrivacyMarker
    {
        private bool escapeContent;

        public XmlPrivacyMarker(bool escapeContent)
        {
            this.escapeContent = escapeContent;
        }

        public object ToCompliantValue(IPrivateDataContainer privateData)
        {
            var stringValue = privateData.GetValue()?.ToString();

            if (privateData.PrivacyLevel == PrivacyLevel.PublicData)
            {
                return stringValue;
            }

            if (this.escapeContent)
            {
                stringValue = XmlUtil.EscapeXmlNodeContent(stringValue);
            }

            return MarkValue(privateData.PrivacyLevel, stringValue);
        }

        /// <summary>
        /// Convert enum value to a name used to mark text output for privacy.
        /// </summary>
        /// <param name="privacyLevel">The <see cref="PrivacyLevel "/> to get a marker for.</param>
        /// <returns>Marker string for a given <see cref="PrivacyLevel"/>.</returns>
        private static string ToMarker(PrivacyLevel privacyLevel)
        {
            // Ascending privacy levels
            switch (privacyLevel)
            {
                case PrivacyLevel.PublicData: return null;
                case PrivacyLevel.OrganizationIdentifiableInformation: return "oii";
                case PrivacyLevel.EndUserIdentifiableInformation: return "euii";

                case PrivacyLevel.CustomerData:
                default:
                    return "cd";
            }
        }

        private static string ToStartMarker(PrivacyLevel privacyLevel)
        {
            switch (privacyLevel)
            {
                case PrivacyLevel.PublicData: return string.Empty;
                default:
                    return $"<{ToMarker(privacyLevel)}>";
            }
        }

        private static string ToEndMarker(PrivacyLevel privacyLevel)
        {
            switch (privacyLevel)
            {
                case PrivacyLevel.PublicData: return string.Empty;
                default:
                    return $"</{ToMarker(privacyLevel)}>";
            }
        }

        private static string MarkValue(PrivacyLevel privacyLevel, string value)
        {
            if (privacyLevel != PrivacyLevel.PublicData)
            {
                return ToStartMarker(privacyLevel) + value + ToEndMarker(privacyLevel);
            }

            return value;
        }
    }
}
