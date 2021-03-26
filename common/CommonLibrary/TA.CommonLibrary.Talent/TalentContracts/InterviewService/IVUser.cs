//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace TA.CommonLibrary.Talent.TalentContracts.InterviewService
{
    using TA.CommonLibrary.Common.Web.Contracts;
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
