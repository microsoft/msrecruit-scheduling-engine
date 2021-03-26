//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;

    [ODataEntity(PluralName = "tasks", SingularName = "task")]
    public class TaskActivity: ActivityPointer
    { 
        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "Task_Annotation")]
        public IList<Annotation> Notes { get; set; }
    }
}
