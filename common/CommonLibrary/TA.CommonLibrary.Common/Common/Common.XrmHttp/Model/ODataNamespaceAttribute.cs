//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public class ODataNamespaceAttribute : Attribute
    {
        public string Namespace { get; set; }
    }
}
