//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ODataEntityAttribute : Attribute
    {
        public string SingularName { get; set; }

        public string PluralName { get; set; }
    }
}
