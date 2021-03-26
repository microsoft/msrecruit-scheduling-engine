//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ServicePlatform.Context
{
    public interface ICorrelationContext
    {
        /// <summary>
        /// SessionId
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// RootActivityId
        /// </summary>
        string RootActivityId { get; }

        /// <summary>
        /// Activity Vector
        /// </summary>
        string ActivityVector { get; }

        /// <summary>
        /// Application name
        /// </summary>
        string Application { get; }

        /// <summary>
        /// Service name
        /// </summary>
        string Service { get; }

        /// <summary>
        /// Package version number
        /// </summary>
        string CodePackageVersion { get; }
    }
}
