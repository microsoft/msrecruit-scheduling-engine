//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "tasks", SingularName = "task")]
    public class TaskActivity: ActivityPointer
    { 
        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "Task_Annotation")]
        public IList<Annotation> Notes { get; set; }
    }
}