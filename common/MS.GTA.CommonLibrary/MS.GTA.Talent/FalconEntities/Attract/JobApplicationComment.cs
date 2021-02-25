//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class JobApplicationComment
    {
        [DataMember(Name = "JobApplicationCommentID", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationCommentID { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        [DataMember(Name = "CreatedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime CreatedDateTime { get; set; }
    }
}
