//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System.Runtime.Serialization;

    public class XrmErrorResponseBody
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "error")]
        public XrmErrorResponseBodyError Error { get; set; }

        public class XrmErrorResponseBodyError
        {
            [DataMember(Name = "code")]
            public string Code { get; set; }

            [DataMember(Name = "message")]
            public string Message { get; set; }

            [DataMember(Name = "innererror")]
            public XrmErrorResponseBodyInnerError InnerError { get; set; }

            public class XrmErrorResponseBodyInnerError
            {
                [DataMember(Name = "message")]
                public string Message { get; set; }

                [DataMember(Name = "type")]
                public string Type { get; set; }

                [DataMember(Name = "stacktrace")]
                public string StackTrace { get; set; }
            }
        }
    }
}
