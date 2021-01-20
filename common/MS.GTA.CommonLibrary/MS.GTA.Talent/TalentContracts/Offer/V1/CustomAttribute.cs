﻿// <copyright file="CustomAttribute.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    /// <summary>
    /// Custom attributes contract for extended attributes for Job, Position, Candidate
    /// </summary>
    public class CustomAttribute
    {
        /// <summary>
        /// Gets or sets name of the attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets value of the attribute
        /// </summary>
        public string Value { get; set; }
    }
}
