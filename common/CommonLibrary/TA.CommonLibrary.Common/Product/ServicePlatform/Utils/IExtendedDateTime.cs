//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.CommonLibrary.ServicePlatform.Utils
{
    public interface IExtendedDateTime
    {
        DateTime UtcNow { get; }
    }
}
