//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "contacts", SingularName = "contact")]
    public class Contact: XrmODataEntity
    {
        [Key]
        [DataMember(Name = "contactid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "emailaddress1")]
        public string EmailAddressPrimary { get; set; }

        [DataMember(Name = "telephone1")]
        public string TelephonePrimary { get; set; }

        [DataMember(Name = "Contact_Annotation")]
        public IList<Annotation> Notes { get; set; }
    }
}
