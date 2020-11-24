//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.GTA.ServicePlatform.Utils
{
    internal sealed class ExtendedDateTime : IExtendedDateTime
    {
        public static IExtendedDateTime Instance { get; } = new ExtendedDateTime();
        private ExtendedDateTime()
        {
        }

        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
