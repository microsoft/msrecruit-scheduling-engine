//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Communication
{
    /// <summary>
    /// Defines segments to be used for unique service uri.
    /// </summary>
    [Flags]
    public enum UniqueServiceUriSegments
    {
        /// <summary>
        /// Do not use unique service uri.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include service name.
        /// </summary>
        ServiceName = 1,

        /// <summary>
        /// Include partition identifier.
        /// </summary>
        Partition = 2,

        /// <summary>
        /// Include replica or instance identifier.
        /// </summary>
        ReplicaOrInstanceId = 4,

        /// <summary>
        /// Include all segments (service name, partition id, replica or instance id)
        /// </summary>
        All = ServiceName | Partition | ReplicaOrInstanceId
    }
}
