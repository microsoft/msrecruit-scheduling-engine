//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonDataService.Common.Internal;
using System;

namespace ServicePlatform.Context
{
    public class CloudEnvironment : IEnvironmentContext
    {
        private const string mdmAccoutVariableName = "D365_Environment_MdmAccount";
        private const string mdmNamespaceVariableName = "D365_Environment_MdmNamespace";
        private const string roleInstanceVariableName = "D365_Environment_RoleInstanceName";
        private const string clusterNameVariableName = "D365_Environment_ClusterName";

        private string applicationName;
        private string serviceName;
        private string codePackageVersion;
        private string partitionId;
        private string replicaOrInstanceId;
        private string listenerName;
        private string mdmAccount = System.Environment.GetEnvironmentVariable(mdmAccoutVariableName, System.EnvironmentVariableTarget.Machine);
        private string mdmNamespace = System.Environment.GetEnvironmentVariable(mdmNamespaceVariableName, System.EnvironmentVariableTarget.Machine);
        private string cluster = System.Environment.GetEnvironmentVariable(clusterNameVariableName, System.EnvironmentVariableTarget.Machine);
        private string roleInstance = System.Environment.GetEnvironmentVariable(roleInstanceVariableName, System.EnvironmentVariableTarget.Machine);

        internal CloudEnvironment()
        {
            applicationName = string.Empty;
            serviceName = string.Empty;
            codePackageVersion = string.Empty;
            partitionId = string.Empty;
            replicaOrInstanceId = string.Empty;
            listenerName = string.Empty;
        }

        /// <summary>
        /// Initialize the environment context with the application and service names.
        /// </summary>
        [Obsolete("This constructor is deprecated. Please use the constructor that also takes codePackageVersion as a parameter.")]
        public CloudEnvironment(string application, string service)
        {
            Contract.CheckValue(application, nameof(application));
            Contract.CheckValue(service, nameof(service));

            applicationName = application;
            serviceName = service;
        }

        /// <summary>
        /// Initialize the environment context with the application and service names.
        /// </summary>
        public CloudEnvironment(string application, string service, string codePackageVersion)
        {
            Contract.CheckValue(application, nameof(application));
            Contract.CheckValue(service, nameof(service));
            Contract.CheckValue(codePackageVersion, nameof(codePackageVersion));

            applicationName = application;
            serviceName = service;
            this.codePackageVersion = codePackageVersion;
        }

        /// <summary>
        /// Initialize the environment context.
        /// </summary>
        public CloudEnvironment(string application, string service, string codePackageVersion, string partitionId, string replicaOrInstanceId, string listenerName)
            : this(application, service, codePackageVersion)
        {
            Contract.CheckValue(partitionId, nameof(partitionId));
            Contract.CheckValue(replicaOrInstanceId, nameof(replicaOrInstanceId));
            Contract.CheckValue(listenerName, nameof(listenerName));

            this.partitionId = partitionId;
            this.replicaOrInstanceId = replicaOrInstanceId;
            this.listenerName = listenerName;
        }

        public string Application
        {
            get { return applicationName; }
        }

        public string Service
        {
            get { return serviceName; }
        }

        public string CodePackageVersion
        {
            get { return codePackageVersion; }
        }

        public string PartitionId
        {
            get { return partitionId; }
        }

        public string ReplicaOrInstanceId
        {
            get { return replicaOrInstanceId; }
        }

        public string ListenerName
        {
            get { return listenerName; }
        }

        public string MdmAccount
        {
            get { return mdmAccount; }
        }

        public string MdmNamespace
        {
            get { return mdmNamespace; }
        }

        public string Cluster
        {
            get { return cluster; }
        }

        public string RoleInstance
        {
            get { return roleInstance; }
        }
    }
}
