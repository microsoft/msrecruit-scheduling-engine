//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Specifies the Data Contract for updating template in multiple template packages
    /// </summary>
    [DataContract]
    public class TemplatePackageUpdate
    {
        /// <summary>
        /// Gets or sets list of template package ids from which template should be removed
        /// </summary>
        [DataMember(Name = "packageIdsToRemoveTemplate", IsRequired = false)]
        public IList<string> PackageIdsToRemoveTemplate { get; set; }

        /// <summary>
        /// Gets or sets list of template package ids to which template should be added
        /// </summary>
        [DataMember(Name = "packageIdsToAddTemplate", IsRequired = false)]
        public IList<string> PackageIdsToAddTemplate { get; set; }
    }
}
