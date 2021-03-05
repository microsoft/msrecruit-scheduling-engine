//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    [ServiceContract(Name = "IOrganizationService", Namespace = "http://schemas.microsoft.com/xrm/2011/Contracts/Services")]
    public interface IOrganizationServiceAsyncDisposable : IOrganizationServiceAsync, IOrganizationService, IDisposable
    {
        #region Copied from IOrganizationService - TODO: remove

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new Guid Create(Entity entity);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new void Delete(string entityName, Guid id);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new OrganizationResponse Execute(OrganizationRequest request);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new Entity Retrieve(string entityName, Guid id, ColumnSet columnSet);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new EntityCollection RetrieveMultiple(QueryBase query);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        new void Update(Entity entity);

        #endregion
    }
}
