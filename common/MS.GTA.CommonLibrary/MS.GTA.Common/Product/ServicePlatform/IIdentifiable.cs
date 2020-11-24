// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform
{
    /// <summary>
    /// Represents an entity with an identity (a name).
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Gets a human-readable name which uniquely identifies the entity.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
