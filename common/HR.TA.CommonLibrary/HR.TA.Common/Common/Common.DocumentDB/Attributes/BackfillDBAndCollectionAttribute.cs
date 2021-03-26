//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.DocumentDB.Attributes
{
    using System;

    /// <summary>
    /// Attribute to specify the collection partition key path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PartitionKeyPathAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="PartitionKeyPathAttribute"/> class.</summary>
        /// <param name="path">The path.</param>
        public PartitionKeyPathAttribute(string path)
        {
            this.Path = path;
        }

        /// <summary>Gets the path.</summary>
        public string Path { get; }
    }
}
