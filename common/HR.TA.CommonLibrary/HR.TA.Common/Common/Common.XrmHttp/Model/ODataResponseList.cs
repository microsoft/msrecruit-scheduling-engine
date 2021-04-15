//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ODataResponseList<T> : IODataResponseList
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }

        [DataMember(Name = "@odata.nextLink")]
        public string ODataNextLink { get; set; }

        [DataMember(Name = "@odata.count")]
        public int? ODataCount { get; set; }

        [DataMember(Name = "value")]
        public IList<T> Result { get; set; }

        IEnumerable<object> IODataResponseList.Result { get => (IEnumerable<object>)this.Result; }
    }
}
