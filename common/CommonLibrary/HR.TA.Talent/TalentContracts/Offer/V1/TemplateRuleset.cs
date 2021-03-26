//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
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
