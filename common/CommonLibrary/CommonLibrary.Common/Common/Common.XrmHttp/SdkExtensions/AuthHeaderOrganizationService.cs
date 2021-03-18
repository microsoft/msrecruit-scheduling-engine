//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    internal class AuthHeaderOrganizationService : IOrganizationServiceAsyncDisposable
    {
        private static readonly ChannelFactory<IOrganizationServiceAsync> ChannelFactory =
            new ChannelFactory<IOrganizationServiceAsync>(
                new BasicHttpsBinding()
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    ReceiveTimeout = TimeSpan.FromMinutes(2),
                    SendTimeout = TimeSpan.FromMinutes(2),
                    ReaderQuotas =
                    {
                        MaxStringContentLength = int.MaxValue,
                        MaxArrayLength = int.MaxValue,
                        MaxBytesPerRead = int.MaxValue,
                        MaxNameTableCharCount = int.MaxValue,
                    },
                },
                null)
            {
                Endpoint =
                {
                    EndpointBehaviors = { new AuthHeaderBehavior() },
                    // TODO: remove once IOrganizationServiceAsync inherits from IOrganizationService and the KnownTypesResolver is made thread-safe
                    Contract = { ContractBehaviors = { new KnownAssemblyAttribute() } },
                },
            };

        private readonly IOrganizationServiceAsync organizationService;
        private readonly Func<Task<string>> getToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthHeaderOrganizationService"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="getToken">The get token.</param>
        /// <param name="logger">The logger.</param>
        private AuthHeaderOrganizationService(string domain, Func<Task<string>> getToken, ILogger logger)
        {
            this.organizationService = ChannelFactory.CreateChannel(new EndpointAddress($"{domain}/XRMServices/2011/Organization.svc/web"));
            ((IClientChannel)this.organizationService).Extensions.Add(new ChannelExtensionInfo() { Logger = logger });
            this.getToken = getToken;
        }

        /// <summary>
        /// Creates the specified domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="getToken">The get token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<IOrganizationServiceAsyncDisposable> Create(string domain, Func<Task<string>> getToken, ILogger logger)
        {
            var organizationService = new AuthHeaderOrganizationService(domain, getToken, logger);
            await organizationService.RefreshToken();
            return organizationService;
        }

        /// <summary>
        /// Associates the asynchronous.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="relatedEntities">The related entities.</param>
        /// <returns></returns>
        public async Task AssociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            await this.RefreshToken();
            await organizationService.AssociateAsync(entityName, entityId, relationship, relatedEntities);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Guid> CreateAsync(Entity entity)
        {
            await this.RefreshToken();
            return await organizationService.CreateAsync(entity);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, Guid id)
        {
            await this.RefreshToken();
            await organizationService.DeleteAsync(entityName, id);
        }

        /// <summary>
        /// Disassociates the asynchronous.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="relatedEntities">The related entities.</param>
        /// <returns></returns>
        public async Task DisassociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            await this.RefreshToken();
            await organizationService.DisassociateAsync(entityName, entityId, relationship, relatedEntities);
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<OrganizationResponse> ExecuteAsync(OrganizationRequest request)
        {
            await this.RefreshToken();
            return await organizationService.ExecuteAsync(request);
        }

        /// <summary>
        /// Retrieves the asynchronous.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="columnSet">The column set.</param>
        /// <returns></returns>
        public async Task<Entity> RetrieveAsync(string entityName, Guid id, ColumnSet columnSet)
        {
            await this.RefreshToken();
            return await organizationService.RetrieveAsync(entityName, id, columnSet);
        }

        /// <summary>
        /// Retrieves the multiple asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public async Task<EntityCollection> RetrieveMultipleAsync(QueryBase query)
        {
            await this.RefreshToken();
            return await organizationService.RetrieveMultipleAsync(query);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task UpdateAsync(Entity entity)
        {
            await this.RefreshToken();
            await organizationService.UpdateAsync(entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Guid Create(Entity entity)
        {
            this.RefreshToken().Wait();
            return organizationService.Create(entity);
        }

        /// <summary>
        /// Retrieves the specified entity name.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="columnSet">The column set.</param>
        /// <returns></returns>
        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            this.RefreshToken().Wait();
            return organizationService.Retrieve(entityName, id, columnSet);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(Entity entity)
        {
            this.RefreshToken().Wait();
            organizationService.Update(entity);
        }

        /// <summary>
        /// Deletes the specified entity name.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The identifier.</param>
        public void Delete(string entityName, Guid id)
        {
            this.RefreshToken().Wait();
            organizationService.Delete(entityName, id);
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public OrganizationResponse Execute(OrganizationRequest request)
        {
            this.RefreshToken().Wait();
            return organizationService.Execute(request);
        }

        /// <summary>
        /// Associates the specified entity name.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="relatedEntities">The related entities.</param>
        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            this.RefreshToken().Wait();
            organizationService.Associate(entityName, entityId, relationship, relatedEntities);
        }

        /// <summary>
        /// Disassociates the specified entity name.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="relatedEntities">The related entities.</param>
        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            this.RefreshToken().Wait();
            organizationService.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        /// <summary>
        /// Retrieves the multiple.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            this.RefreshToken().Wait();
            return organizationService.RetrieveMultiple(query);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IClientChannel)this.organizationService).Dispose();
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshToken()
        {
            var authHeaderInfo = ((IClientChannel)organizationService).Extensions.Find<ChannelExtensionInfo>();
            authHeaderInfo.Token = await getToken();
        }

        /// <summary>
        /// Auth Header behaviour class
        /// </summary>
        /// <seealso cref="System.ServiceModel.Description.IEndpointBehavior" />
        class AuthHeaderBehavior : IEndpointBehavior
        {
            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.ClientMessageInspectors.Add(new MessageInspector());
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            }

            public void Validate(ServiceEndpoint endpoint)
            {
            }

            class MessageInspector : IClientMessageInspector
            {
                public void AfterReceiveReply(ref Message reply, object correlationState)
                {
                    if (reply.Properties.TryGetValue(HttpResponseMessageProperty.Name, out var prop)
                        && prop is HttpResponseMessageProperty httpResponseMessage
                        && correlationState is IClientChannel channel)
                    {
                        var extensionInfo = channel.Extensions.Find<ChannelExtensionInfo>();
                        extensionInfo?.Logger?.LogInformation($"AuthHeaderOrganizationService: got {httpResponseMessage.StatusCode} ({httpResponseMessage.StatusDescription}) response REQ_ID={httpResponseMessage.Headers.Get("REQ_ID")} x-ms-service-request-id={httpResponseMessage.Headers.Get("x-ms-service-request-id")} AuthActivityId={httpResponseMessage.Headers.Get("AuthActivityId")}");
                    }
                }

                public object BeforeSendRequest(ref Message request, IClientChannel channel)
                {
                    var extensionInfo = channel.Extensions.Find<ChannelExtensionInfo>();
                    request.Properties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty
                    {
                        Headers =
                        {
                            { HttpRequestHeader.Authorization, "Bearer " + extensionInfo.Token },
                        }
                    };
                    return channel;
                }
            }
        }

        class ChannelExtensionInfo : IExtension<IContextChannel>
        {
            public string Token { get; set; }

            public ILogger Logger { get; set; }

            public void Attach(IContextChannel owner)
            {
            }

            public void Detach(IContextChannel owner)
            {
            }
        }

        // TODO: copied from the SDK, remove once IOrganizationServiceAsync inherits from IOrganizationService and the KnownTypesResolver is made thread-safe
        class KnownAssemblyAttribute : IContractBehavior
        {
            LockedKnownTypesResolver resolver;

            public KnownAssemblyAttribute()
            {
                resolver = new LockedKnownTypesResolver();
            }

            public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                CreateMyDataContractSerializerOperationBehaviors(contractDescription);
            }

            public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
            {
                CreateMyDataContractSerializerOperationBehaviors(contractDescription);
            }

            public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
            {
            }

            private void CreateMyDataContractSerializerOperationBehaviors(ContractDescription contractDescription)
            {
                foreach (var operation in contractDescription.Operations)
                {
                    CreateMyDataContractSerializerOperationBehavior(operation);
                }
            }

            private void CreateMyDataContractSerializerOperationBehavior(OperationDescription operation)
            {
                DataContractSerializerOperationBehavior dataContractSerializerOperationbehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dataContractSerializerOperationbehavior != null)
                {
                    dataContractSerializerOperationbehavior.DataContractResolver = this.resolver;
                }
            }
        }
    }
}
