//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.Common.DocumentDB.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CommonLibrary.Talent.FalconEntities.Attract
{
    [DataContract]
    public class TalentTag : DocDbEntity
    {
        [DataMember(Name = "TalentTagID", EmitDefaultValue = false, IsRequired = false)]
        public string TalentTagID { get; set; }

        [DataMember(Name = "Tag", EmitDefaultValue = false, IsRequired = false)]
        public string Tag { get; set; }

        [DataMember(Name = "TalentTagAssociations", EmitDefaultValue = false, IsRequired = false)]
        public IList<TalentTagAssociation> TalentTagAssociations { get; set; }
    }
}
