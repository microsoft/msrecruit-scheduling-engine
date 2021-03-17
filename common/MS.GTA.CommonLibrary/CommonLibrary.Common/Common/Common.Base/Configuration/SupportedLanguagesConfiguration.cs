//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Configuration
{
    using System;
    using System.Globalization;

    /// <summary>The supported languages configuration.</summary>
    public class SupportedLanguagesConfiguration
    {
        /// <summary>Gets or sets the default language code.</summary>
        public static string DefaultLanguage = "en-us";

        /// <summary>Gets or sets the supported language codes - this is a semi-colon delimited string.</summary>
        public static string SupportedLanguages = "en-us;en-gb";

        /// <summary>Map language code to supported culture code.</summary>
        /// <param name="langCode">The language code.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static CultureInfo GetCultureInfo(string langCode)
        {
            var baseLanguage = DefaultLanguage;
            if (SupportedLanguages.IndexOf(langCode, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                baseLanguage = langCode;
            }

            return new CultureInfo(baseLanguage);
        }
    }
}
