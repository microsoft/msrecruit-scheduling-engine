// <copyright file="TemplateRuleset.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;

    /// <summary>
    /// Template rule set
    /// </summary>
    public class TemplateRuleset : BaseDocumentType
    {
        /// <summary>Gets or sets the template Id.</summary>
        public string TemplateId { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        public string TenantID { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        public string EnvironmentID { get; set; }

        /// <summary>Gets or sets the rules data.</summary>
        public List<string> Rulesets { get; set; }
    }
}
