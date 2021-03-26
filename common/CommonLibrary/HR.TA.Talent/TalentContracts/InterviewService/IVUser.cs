//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace HR.TA.Talent.TalentContracts.InterviewService
{
    using HR.TA.Common.Web.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class representing a list of users with roles
    /// </summary>
    [DataContract]
    public class IVUser
    {
        [DataMember(Name = "ivperson", IsRequired = true)]
        public IVPerson Person { get; set; }

        [DataMember(Name = "ivroles", IsRequired = true)]
        public IList<IVApplicationRole> Roles { get; set; }
    }
}
