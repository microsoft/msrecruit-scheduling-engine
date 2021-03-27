//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.Enum
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
