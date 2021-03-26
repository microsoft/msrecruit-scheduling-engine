//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace TA.CommonLibrary.Common.BapClient.Contracts
{
    /// <summary>
    /// The principal contract
    /// </summary>
    public class Principal
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the principal display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the principal email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the principal object type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets AAD Tenant ID.
        /// </summary>
        public string TenantId { get; set; }
    }
}
