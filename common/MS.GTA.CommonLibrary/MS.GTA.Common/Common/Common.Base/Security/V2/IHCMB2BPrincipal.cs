namespace MS.GTA.Common.Base.Security.V2
{
    /// <summary>The HCMB2BPrincipal interface.</summary>
    public interface IHCMB2BPrincipal : IHCMPrincipal
    {
        /// <summary>Gets The users name.</summary>
        string Email { get; }

        /// <summary>
        /// Gets the family name
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Gets the given name
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// Gets The users IP address.
        /// </summary>
        string IpAddress { get; }

        /// <summary>
        /// Gets The users name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Gets the unique name, likely an email address.
        /// </summary>
        string UniqueName { get; }
    }
}
