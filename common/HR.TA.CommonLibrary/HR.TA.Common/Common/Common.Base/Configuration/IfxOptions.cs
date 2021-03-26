//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>The IFX options.</summary>
    [SettingsSection("Ifx")]
    public class IfxOptions
    {
        /// <summary>Gets or sets the activity metric namespace.</summary>
        public string ActivityMetricNamespace { get; set; }

        /// <summary>Gets or sets the custom metric namespace.</summary>
        public string CustomMetricNamespace { get; set; }

        /// <summary>Gets or sets a value indicating whether enable custom trace properties.</summary>
        public bool EnableCustomTraceProperties { get; set; }

        /// <summary>Gets or sets the MDM account.</summary>
        public string MdmAccount { get; set; }

        /// <summary>The to IFX logger options.</summary>
        /// <returns>The <see cref="IfxLoggerOptions"/>.</returns>
        //public IfxLoggerOptions ToIfxLoggerOptions()
        //{
        //    return new IfxLoggerOptions
        //        {
        //            ActivityMetricNamespace = this.ActivityMetricNamespace,
        //            CustomMetricNamespace = this.CustomMetricNamespace,
        //            EnableCustomTraceProperties = this.EnableCustomTraceProperties,
        //            MdmAccount = this.MdmAccount
        //        };
        //}
    }
}
