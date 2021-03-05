//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml;
    using System.Xml.Linq;

    public struct FetchXmlAttribute
    {
        public ODataField Field { get; set; }

        public string Alias { get; set; }

        public bool GroupBy { get; set; }

        public FetchXmlAggregateType? AggregateType { get; set; }

        public bool Distinct { get; set; }

        public FetchXmlDateGrouping? DateGrouping { get; set; }

        public override string ToString() => this.ToXElement().ToString(SaveOptions.DisableFormatting);

        public XElement ToXElement()
        {
            var element = new XElement(
                "attribute",
                new XAttribute("name", this.Field.ToFetchXmlFieldName()));

            element.SetAttributeValue("alias", this.Alias);

            if (this.GroupBy)
            {
                element.SetAttributeValue("groupby", "true");
            }

            if (this.AggregateType != null)
            {
                element.SetAttributeValue("aggregate", this.AggregateType.ToString().ToLowerInvariant());
            }

            if (this.Distinct)
            {
                element.SetAttributeValue("distinct", "true");
            }

            switch (this.DateGrouping)
            {
                case null:
                    break;
                case FetchXmlDateGrouping.FiscalPeriod:
                    element.SetAttributeValue("dategrouping", "fiscal-period");
                    break;
                case FetchXmlDateGrouping.FiscalYear:
                    element.SetAttributeValue("dategrouping", "fiscal-year");
                    break;
                default:
                    element.SetAttributeValue("dategrouping", this.DateGrouping.ToString().ToLowerInvariant());
                    break;
            }

            return element;
        }
    }
}
