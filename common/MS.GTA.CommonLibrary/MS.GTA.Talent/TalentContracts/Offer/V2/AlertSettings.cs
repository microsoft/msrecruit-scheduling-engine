//----------------------------------------------------------------------------
// <copyright file="AlertSettings.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Alert Dialog Setting contract
    /// </summary>
    [DataContract]
    public class AlertSettings
    {
        /// <summary>
        /// Gets or sets Name of Setting.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether true or false.
        /// </summary>
        [DataMember(Name = "value", IsRequired = false, EmitDefaultValue = true)]
        public bool Value { get; set; }
    }
}
