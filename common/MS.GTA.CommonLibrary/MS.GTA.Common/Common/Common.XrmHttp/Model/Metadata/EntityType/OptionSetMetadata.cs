//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    public class OptionSetMetadata : OptionSetMetadataBase
    {
        [DataMember(Name = "Options")]
        public IList<OptionMetadata> Options { get; set; }

        [DataMember(Name = "ParentOptionSetName")]
        public string ParentOptionSetName { get; set; }

        public string GetOptionValueLabelForCulture(int value, CultureInfo cultureInfo)
        {
            return this.Options?.FirstOrDefault(o => o?.Value == value)?.Label?.GetLabelForCulture(cultureInfo);
        }

        public IList<(int, string)> GetOptionLabelsForCulture(CultureInfo cultureInfo)
        {
            return this.Options
                ?.Where(o => o?.Value != null)
                .Select(o => (o.Value.Value, o.Label?.GetLabelForCulture(cultureInfo)))
                .ToArray();
        }
    }
}