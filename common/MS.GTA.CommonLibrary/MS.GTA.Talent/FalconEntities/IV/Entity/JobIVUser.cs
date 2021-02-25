//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.IV.Entity
{
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class JobIVUser : DocDbEntity
    {
        [DataMember(Name = "ivperson")]
        public IVPerson Person { get; set; }

        [DataMember(Name = "ivroles")]
        public IList<IVApplicationRole> Roles { get; set; }

        [DataMember(Name = "ivfirstlogin", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? firsttimelogin { get; set; }
    }
}

