//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    // TODO: inherit from IOrganizationService from the SDK
    // blocked by https://github.com/dotnet/wcf/issues/3384

    //
    // Summary:
    //     Provides programmatic access to the metadata and data for an organization.
    [ServiceContract(Name = "IOrganizationService", Namespace = "http://schemas.microsoft.com/xrm/2011/Contracts/Services")]
    public interface IOrganizationServiceAsync
    {
        //
        // Summary:
        //     Creates a link between records.
        //
        // Parameters:
        //   relatedEntities:
        //     Type: Microsoft.Xrm.Sdk.EntityReferenceCollection. property_relatedentities to
        //     be associated.
        //
        //   relationship:
        //     Type: Microsoft.Xrm.Sdk.Relationship. property_relationshipname to be used to
        //     create the link.
        //
        //   entityName:
        //     Type: Returns_String. property_logicalname that is specified in the entityId
        //     parameter.
        //
        //   entityId:
        //     Type: Returns_Guid. property_entityid to which the related records are associated.
        [OperationContract]
        Task AssociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        //
        // Summary:
        //     Creates a record.
        //
        // Parameters:
        //   entity:
        //     Type: Microsoft.Xrm.Sdk.Entity. An entity instance that contains the properties
        //     to set in the newly created record.
        //
        // Returns:
        //     Type:Returns_Guid The ID of the newly created record.
        [OperationContract]
        Task<Guid> CreateAsync(Entity entity);

        //
        // Summary:
        //     Deletes a record.
        //
        // Parameters:
        //   id:
        //     Type: Returns_Guid. property_entityid that you want to delete.
        //
        //   entityName:
        //     Type: Returns_String. property_logicalname that is specified in the entityId
        //     parameter.
        [OperationContract]
        Task DeleteAsync(string entityName, Guid id);

        //
        // Summary:
        //     Deletes a link between records.
        //
        // Parameters:
        //   relatedEntities:
        //     Type: Microsoft.Xrm.Sdk.EntityReferenceCollection. property_relatedentities to
        //     be disassociated.
        //
        //   relationship:
        //     Type: Microsoft.Xrm.Sdk.Relationship. property_relationshipname to be used to
        //     remove the link.
        //
        //   entityName:
        //     Type: Returns_String. property_logicalname that is specified in the entityId
        //     parameter.
        //
        //   entityId:
        //     Type: Returns_Guid. property_entityid from which the related records are disassociated.
        [OperationContract]
        Task DisassociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        //
        // Summary:
        //     Executes a message in the form of a request, and returns a response.
        //
        // Parameters:
        //   request:
        //     Type: Microsoft.Xrm.Sdk.OrganizationRequest. A request instance that defines
        //     the action to be performed.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.OrganizationResponseThe response from the request. You
        //     must cast the return value of this method to the specific instance of the response
        //     that corresponds to the Request parameter.
        [OperationContract]
        Task<OrganizationResponse> ExecuteAsync(OrganizationRequest request);

        //
        // Summary:
        //     Retrieves a record.
        //
        // Parameters:
        //   id:
        //     Type: Returns_Guid. property_entityid that you want to retrieve.
        //
        //   columnSet:
        //     Type: Microsoft.Xrm.Sdk.Query.ColumnSet. A query that specifies the set of columns,
        //     or attributes, to retrieve.
        //
        //   entityName:
        //     Type: Returns_String. property_logicalname that is specified in the entityId
        //     parameter.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Entity The requested entity.
        [OperationContract]
        Task<Entity> RetrieveAsync(string entityName, Guid id, ColumnSet columnSet);

        //
        // Summary:
        //     Retrieves a collection of records.
        //
        // Parameters:
        //   query:
        //     Type: Microsoft.Xrm.Sdk.Query.QueryBase. A query that determines the set of records
        //     to retrieve.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.EntityCollectionThe collection of entities returned from
        //     the query.
        [OperationContract]
        Task<EntityCollection> RetrieveMultipleAsync(QueryBase query);

        //
        // Summary:
        //     Updates an existing record.
        //
        // Parameters:
        //   entity:
        //     Type: Microsoft.Xrm.Sdk.Entity. An entity instance that has one or more properties
        //     set to be updated in the record.
        [OperationContract]
        Task UpdateAsync(Entity entity);

        #region Copied from IOrganizationService - TODO: remove

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        Guid Create(Entity entity);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        void Delete(string entityName, Guid id);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        OrganizationResponse Execute(OrganizationRequest request);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        Entity Retrieve(string entityName, Guid id, ColumnSet columnSet);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        EntityCollection RetrieveMultiple(QueryBase query);

        [FaultContract(typeof(OrganizationServiceFault))]
        [OperationContract]
        void Update(Entity entity);

        #endregion
    }
}
