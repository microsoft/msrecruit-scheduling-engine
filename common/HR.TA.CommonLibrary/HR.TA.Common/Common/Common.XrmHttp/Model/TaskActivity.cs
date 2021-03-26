//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "tasks", SingularName = "task")]
    public class TaskActivity: ActivityPointer
    { 
        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "Task_Annotation")]
        public IList<Annotation> Notes { get; set; }
    }
}
