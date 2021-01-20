// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IBapServiceClient.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace MS.GTA.Common.BapClient
{
    using Contracts;
    using Contracts.XRM;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Client for performing operations on the BAP environments.
    /// </summary>
    public interface IBapServiceClient
    {
        /// <summary>The ensure default bap environment async.</summary>
        /// <param name="tenantId">The tenant Id.</param>
        /// <returns>The <see cref="Task{EnvironmentDefinition}"/>.</returns>
        Task<EnvironmentDefinition> EnsureDefaultBapEnvironment(string tenantId);
        
        /// <summary>The get bap environments.</summary>
        /// <param name="tenantId">The tenant Id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<EnvironmentDefinition>> GetBapEnvironments(string tenantId);

        /// <summary>The get bap environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<EnvironmentDefinition> GetBapEnvironment(string tenantId, string bapEnvironmentId);

        /// <summary>Get a bap XRM environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<XRMEnvironmentDefinition> GetXRMEnvironment(string tenantId, string bapEnvironmentId);

        /// <summary>Check user has permission in given tenant and environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <param name="userObjectId">The user object id.</param>
        /// <param name="permissionName">The permission name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> CheckUserPermission(string tenantId, string bapEnvironmentId, string userObjectId, string permissionName);
    }
}