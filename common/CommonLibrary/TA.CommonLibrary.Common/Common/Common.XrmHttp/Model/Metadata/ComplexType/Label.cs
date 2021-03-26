//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model.Metadata
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    public class Label
    {
        [DataMember(Name = "LocalizedLabels")]
        public IList<LocalizedLabel> LocalizedLabels { get; set; }

        [DataMember(Name = "UserLocalizedLabel")]
        public LocalizedLabel UserLocalizedLabel { get; set; }

        public string GetLabelForCulture(CultureInfo cultureInfo)
        {
            if (this.LocalizedLabels != null && cultureInfo != null)
            {
                var labelCultureInfos = this.LocalizedLabels
                    .Where(l =>
                        l?.LanguageCode != null
                        && !string.IsNullOrEmpty(l.Label))
                    .Select(l => (Label: l.Label, CultureInfo: CultureInfo.GetCultureInfo(l.LanguageCode.Value)))
                    .ToArray();

                // For background on the invariant culture and parent cultures, see:
                // https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo?view=netframework-4.7.2#Invariant
                while (cultureInfo != CultureInfo.InvariantCulture)
                {
                    // Find an exact match, if any.
                    var label = labelCultureInfos.FirstOrDefault(l => l.CultureInfo.Name.Equals(cultureInfo.Name));
                    if (label.Label != null)
                    {
                        return label.Label;
                    }

                    // If the user localized label is a related language to the requested language, use it.
                    if (this.UserLocalizedLabel?.LanguageCode != null
                        && !string.IsNullOrEmpty(this.UserLocalizedLabel.Label)
                        && IsSameOrMoreSpecificLanguageThan(CultureInfo.GetCultureInfo(this.UserLocalizedLabel.LanguageCode.Value), cultureInfo))
                    {
                        return this.UserLocalizedLabel.Label;
                    }

                    // Handle picking some arbitrary variant of the specified language instead of giving up.
                    // e.g. User picked "en", but only "en-us" is available.
                    label = labelCultureInfos.FirstOrDefault(l => IsSameOrMoreSpecificLanguageThan(l.CultureInfo, cultureInfo));
                    if (label.Label != null)
                    {
                        return label.Label;
                    }

                    // Try the parent culture.
                    cultureInfo = cultureInfo.Parent;
                }
            }

            return this.UserLocalizedLabel?.Label;
        }

        private bool IsSameOrMoreSpecificLanguageThan(CultureInfo cultureInfo, CultureInfo otherCultureInfo)
        {
            while (cultureInfo != CultureInfo.InvariantCulture)
            {
                if (cultureInfo.Equals(otherCultureInfo))
                {
                    return true;
                }

                cultureInfo = cultureInfo.Parent;
            }

            return false;
        }
    }
}
