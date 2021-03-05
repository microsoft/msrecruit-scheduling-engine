//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.EnvironmentSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Common.EnvironmentSettings.Contracts;

    /// <summary>The EnvironmentMapRepository interface.</summary>
    public interface IEnvironmentMapRepository
    {
        /// <summary>Get environment map.</summary>
        /// <param name="tenantId">The company tenant Id.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> GetEnvironmentMapWithLogo(string tenantId);

        /// <summary>Get environment map.</summary>
        /// <param name="alias">The company alias.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> GetEnvironmentMapWithLogoByAlias(string alias);

        /// <summary>Get environment map.</summary>
        /// <param name="environmentId">The environment id.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> GetEnvironmentMapByEnvironmentId(string environmentId);

        /// <summary>Get environment map list.</summary>
        /// <param name="tenantId">The company tenantId id.</param>
        /// <returns>List of <see cref="EnvironmentMap"/>s.</returns>
        Task<IEnumerable<EnvironmentMap>> GetEnvironmentMapsByTenantId(string tenantId);

        /// <summary>Get environment map.</summary>
        /// <param name="alias">The company name.</param>
        /// <param name="aliasId">The alias Id.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> GetEnvironmentMapForClientAsync(string alias, string aliasId);

        /// <summary>Create an environment map.</summary>
        /// <param name="environmentMap">The environment map.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> CreateEnvironmentMap(EnvironmentMap environmentMap);

        /// <summary>Get or Create an environment map.</summary>
        /// <param name="environmentMap">The environment map.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> GetOrCreateEnvironmentMap(EnvironmentMap environmentMap);

        /// <summary>Update the environment map.</summary>
        /// <param name="id">The source environment ID.</param>
        /// <param name="environmentMap">The environment map.</param>
        /// <returns>The <see cref="EnvironmentMap"/>.</returns>
        Task<EnvironmentMap> UpdateEnvironmentMapAsync(string id, EnvironmentMap environmentMap);

        /// <summary>Get any environment maps with an environment id matching an environment id in the given list</summary>
        /// <param name="environmentIds">The environment ids.</param>
        /// <param name="chunkSize">The number of environment ids to include per query</param>
        /// <returns>List of <see cref="EnvironmentMap"/>s.</returns>
        Task<IEnumerable<EnvironmentMap>> GetEnvironmentMapsById(IEnumerable<string> environmentIds, int chunkSize);
    }
}
