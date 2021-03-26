//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;

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
