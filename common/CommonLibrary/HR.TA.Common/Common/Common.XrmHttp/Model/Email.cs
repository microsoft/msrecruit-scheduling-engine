//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "emails", SingularName = "email")]
    public class Email : ActivityPointer
    {
       
        [DataMember(Name = "emailtrackingid")]
        public Guid? EmailTrackingId { get; set; }

        [DataMember(Name = "torecipients")]
        public string ToRecipients { get; set; }

        [DataMember(Name = "sender")]
        public string Sender { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "Email_Annotation")]
        public IList<Annotation> Notes { get; set; }
    }
}
