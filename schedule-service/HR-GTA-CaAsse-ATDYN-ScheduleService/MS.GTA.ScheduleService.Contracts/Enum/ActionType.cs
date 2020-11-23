// <copyright file="ActionType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract for Schedule Action Type
    /// </summary>
    [DataContract]
    public enum ActionType
    {
        /// <summary>Create</summary>
        Create,

        /// <summary>Update</summary>
        Update,

        /// <summary>Delete</summary>
        Delete,

        /// <summary>Process</summary>
        Process,
    }
}
