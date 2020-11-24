//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CheckFileInfo.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Selected desired properties based off definition at <see cref="https://wopirest.readthedocs.io/en/latest/files/CheckFileInfo.html#checkfileinfo"/>
    /// </summary>
    [DataContract]
    public class CheckFileInfo
    {
        /// <summary>
        /// Gets or sets the name of the base file.
        /// </summary>
        /// <value>The name of the base file.</value>
        [DataMember(Name = "BaseFileName", IsRequired = true)]
        public string BaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>The owner id.</value>
        [DataMember(Name = "OwnerId", IsRequired = true)]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        [DataMember(Name = "Size", IsRequired = true)]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [DataMember(Name = "Version", IsRequired = true)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the user id of the currently viewing user.
        /// </summary>
        /// <value>The version.</value>
        [DataMember(Name = "UserId", IsRequired = true)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user friendly name for UI display purposes
        /// </summary>
        [DataMember(Name = "UserFriendlyName", IsRequired = false, EmitDefaultValue = false)]
        public string UserFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has readonly file access
        /// </summary>
        [DataMember(Name = "ReadOnly", IsRequired = false, EmitDefaultValue = false)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI client allows users to edit files
        /// </summary>
        [DataMember(Name = "WebEditingDisabled", IsRequired = false, EmitDefaultValue = false)]
        public bool WebEditingDisabled { get; set; }

        /// <summary>
        /// Gets or sets the user-accessible URI to the file; this allows the user to download a copy of the file.
        /// </summary>
        [DataMember(Name = "DownloadUrl", IsRequired = false, EmitDefaultValue = false)]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Gets or sets the fileUrl that the WOPIClient may use to access the file
        /// </summary>
        [DataMember(Name = "FileUrl", IsRequired = false, EmitDefaultValue = false)]
        public string FileUrl { get; set; }

        /// <summary>
        /// Gets or sets the URI to a web page that provides an embeddable viewing experience
        /// </summary>
        [DataMember(Name = "HostEmbeddedViewUrl", IsRequired = false, EmitDefaultValue = false)]
        public string HostEmbeddedViewUrl { get; set; }

        /// <summary>
        /// Gets or sets the string that indicates the brand name of the host.
        /// </summary>
        [DataMember(Name = "BreadcrumbBrandName", IsRequired = false, EmitDefaultValue = false)]
        public string BreadcrumbBrandName { get; set; }

        /// <summary>
        /// Gets or sets the uri to the web page of the brand
        /// </summary>
        [DataMember(Name = "BreadcrumbBrandUrl", IsRequired = false, EmitDefaultValue = false)]
        public string BreadcrumbBrandUrl { get; set; }

        /// <summary>
        /// Gets or sets the string that indicates the file name
        /// </summary>
        [DataMember(Name = "BreadcrumbDocName", IsRequired = false, EmitDefaultValue = false)]
        public string BreadcrumbDocName { get; set; }

        /// <summary>
        /// Gets or sets the string that indicates the name of the folder containing the file
        /// </summary>
        [DataMember(Name = "BreadcrumbFolderName", IsRequired = false, EmitDefaultValue = false)]
        public string BreadcrumbFolderName { get; set; }

        /// <summary>
        /// Gets or sets the uri to the location representing the folder location
        /// </summary>
        [DataMember(Name = "BreadcrumbFolderUrl", IsRequired = false, EmitDefaultValue = false)]
        public string BreadcrumbFolderUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI client should disable print
        /// </summary>
        [DataMember(Name = "DisablePrint", IsRequired = false, EmitDefaultValue = false)]
        public bool DisablePrint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI Client should disable machine translations
        /// </summary>
        [DataMember(Name = "DisableTranslation", IsRequired = false, EmitDefaultValue = false)]
        public bool DisableTranslation { get; set; }

        /// <summary>
        /// Gets or sets the file extension; must begin with a '.'; if value is missing, extension
        /// will be parsed from the BaseFileName property
        /// </summary>
        [DataMember(Name = "FileExtension", IsRequired = false, EmitDefaultValue = false)]
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the string representing the last modification time of the file
        /// </summary>
        [DataMember(Name = "LastModifiedTime", IsRequired = false, EmitDefaultValue = false)]
        public string LastModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI client should restrict user actions on the file
        /// </summary>
        [DataMember(Name = "RestrictedWebViewOnly", IsRequired = false, EmitDefaultValue = false)]
        public bool RestrictedWebViewOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users can create new files on the WOPI Host
        /// </summary>
        [DataMember(Name = "UserCanNotWriteRelative", IsRequired = false, EmitDefaultValue = true)]
        public bool UserCanNotWriteRelative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has permission to alter the file
        /// </summary>
        [DataMember(Name = "UserCanWrite", IsRequired = false, EmitDefaultValue = true)]
        public bool UserCanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI host supports the PutFile and PutRelativeFile WOPI operations
        /// </summary>
        [DataMember(Name = "SupportsUpdate", IsRequired = false, EmitDefaultValue = true)]
        public bool SupportsUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the WOPI host supports the Lock, Unlock, RefreshLock and UnlockAndRelock WOPI operations
        /// </summary>
        [DataMember(Name = "SupportsLocks", IsRequired = false, EmitDefaultValue = true)]
        public bool SupportsLocks { get; set; }

        /// <summary>
        /// Gets or sets the 256 bit SHA-2-encoded hash of the file contents, as a Base64-encoded string.
        /// </summary>
        [DataMember(Name = "SHA256", IsRequired = false, EmitDefaultValue = true)]
        public string SHA256 { get; set; }        
    }
}
