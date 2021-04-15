//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HR.TA.Common.CommonDataModels
{
    /// <summary>
    /// Custom Attribute.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    [ExcludeFromCodeCoverage]
    public class ExcludeFromResponse : Attribute
    {
    }
}
