//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n


namespace CommonLibrary.Common.Base.ServiceContext
{
    using CommonLibrary.Common.Contracts;

    /// <summary>The HCM ServiceContext interface.</summary>
    public interface IHCMServiceContext
    {
        /// <summary>Gets or sets the environment mode.</summary>
        EnvironmentMode EnvironmentMode { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        string EnvironmentId { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        string TenantId { get; set; }

        /// <summary>Gets or sets the token value.</summary>
        string InvitationToken { get; set; }

        /// <summary>Gets or sets the object id value.</summary>
        string ObjectId { get; set; }

        /// <summary>Gets or sets the falcon database id.</summary>
        string FalconDatabaseId { get; set; }

        /// <summary>Gets or sets the falcon resource name.</summary>
        string FalconResourceName { get; set; }

        /// <summary>Gets or sets the falcon container id.</summary>
        string FalconOfferContainerId { get; set; }

        /// <summary>Gets or sets the falcon common container identifier.</summary>
        /// <value>The falcon common container identifier.</value>
        string FalconCommonContainerId { get; set; }

        /// <summary>Gets or sets the XRM instance API uri.</summary>
        string XRMInstanceApiUri { get; set; }

        /// <summary>Gets or sets the Root activity id.</summary>
        string RootActivityId { get; set; }
        
        /// <summary>Gets or sets the Session id.</summary>
        string SessionId { get; set; }

        /// <summary>Gets or sets whether the Attract service endpoint is configured or not.</summary>
        bool IsAttractServiceEndpointConfigured { get; set; }

        /// <summary>
        /// Gets or sets the User Id
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// Gets or sets workonbehalfuserid
        /// </summary>
        string WorkOnBehalfUserId { get; set; }

        /// <summary>
        /// This is a get only property if the user is wob autheticated.
        /// </summary>
        bool isWobAuthenticated { get; }

        /// <summary>
        /// This will get the email id for the context user
        /// </summary>
        string Email { get; set; }
    }
}

