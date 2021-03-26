//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum that tells about Add Update or Delete action. 
    /// </summary>
    [DataContract]
    public enum UserAction
    {
        /// <summary>
        /// No Action is taken in entity 
        /// </summary>
        None,

        /// <summary>
        /// Newly added entity 
        /// </summary>
        Add,

        /// <summary>
        /// Update entity
        /// </summary>
        Update,

        /// <summary>
        /// Delete entity
        /// </summary>
        Delete
    }
}