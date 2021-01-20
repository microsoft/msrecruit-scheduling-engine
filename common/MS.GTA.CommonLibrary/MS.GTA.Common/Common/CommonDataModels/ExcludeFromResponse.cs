using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MS.GTA.Common.CommonDataModels
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
