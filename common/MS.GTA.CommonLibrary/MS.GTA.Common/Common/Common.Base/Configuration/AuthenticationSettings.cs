// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AuthenticationSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Configuration
{
    using System.Collections.Generic;

    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>The auth settings.</summary>
    [SettingsSection("Auth")]
    public class AuthSettings
    {
        /// <summary>Gets or sets the authentication.</summary>
        public AuthenticationSettings Authentication { get; set; }

        /// <summary>Gets or sets the authorization.</summary>
        public AuthorizationSettings Authorization { get; set; }
    }

    /// <summary>The authentication settings.</summary>
    [SettingsSection("Auth:Authentication")]
    public class AuthenticationSettings
    {
        /// <summary>Gets or sets the client id.</summary>
        public string ClientId { get;set; }

        /// <summary>Gets or sets the authority.</summary>
        public string Authority { get;set; }
    }

    /// <summary>The authorization settings.</summary>
    [SettingsSection("Auth:Authorization")]
    public class AuthorizationSettings
    {
        /// <summary>Gets or sets the users authorized for given policies.</summary>
        public IList<User> Users { get; set; } = new List<User>();

        /// <summary>Gets or sets the applications authorized for given policies.</summary>
        public IList<Application> Applications { get; set; } = new List<Application>();

        /// <summary>Gets or sets the groups authorized for given policies.</summary>
        public IList<Group> Groups { get; set; } = new List<Group>();

        /// <summary>Gets or sets the available policies.</summary>
        public string[] AvailablePolicies { get; set; } = { };

        /// <summary>Gets or sets the settings to determine what policies all users are authorized for.</summary>
        public UserSettings AllUsers { get; set; } = new UserSettings();
    }

    /// <summary>The application.</summary>
    public class Application
    {
        /// <summary>Gets or sets the allowed tenants to authenticate.</summary>
        public string[] AllowedTenantsToAuthenticate { get; set; }

        /// <summary>Gets or sets the allowed policies.</summary>
        public string[] AllowedPolicies { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the id.</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }
    }

    /// <summary>The group.</summary>
    public class Group
    {
        /// <summary>Gets or sets the group name.</summary>
        public string GroupName { get; set; }

        /// <summary>Gets or sets the group id.</summary>
        public string GroupId { get; set; }

        /// <summary>Gets or sets the allowed policies.</summary>
        public IList<string> AllowedPolicies { get; set; }
    }

    /// <summary>The user.</summary>
    public class User
    {
        /// <summary>Gets or sets the allowed policies.</summary>
        public IEnumerable<string> AllowedPolicies { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the id.</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }
    }

    /// <summary>The user settings.</summary>
    public class UserSettings
    {
        /// <summary>Gets or sets the allowed policies.</summary>
        public IList<string> AllowedPolicies { get; set; }

        /// <summary>Gets or sets the authenticate with applications.</summary>
        public IList<string> AuthenticateWithApplications { get; set; }
    }
}