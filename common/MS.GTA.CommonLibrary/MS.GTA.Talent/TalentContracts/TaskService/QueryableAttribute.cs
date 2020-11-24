//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="QueryableAttribute.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Talent.TalentContracts.TaskService
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryableAttribute : Attribute
    {
        public string MappingName { get; set; }

        public QueryableAttribute(string mappingName)
        {
            this.MappingName = mappingName;
        }
    }
}
