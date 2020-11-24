// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System.Collections.Generic;

    public interface IODataResponseList
    {
        string ODataContext { get; set; }

        string ODataNextLink { get; set; }

        int? ODataCount { get; set; }

        IEnumerable<object> Result { get; }
    }
}
