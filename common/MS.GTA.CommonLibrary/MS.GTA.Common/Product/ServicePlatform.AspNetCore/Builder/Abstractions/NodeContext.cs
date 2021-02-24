//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Fabric;

namespace MS.GTA.ServicePlatform.AspNetCore.Builder.Abstractions
{
    /// <summary>
    /// The node context interface, abstracting the implementation of retrieving 
    /// the node name of executing code.
    /// </summary>
    public interface INodeContext
    {
        /// <summary>
        /// Gets the current node name.
        /// </summary>
        /// <returns>Returns the string name of the current node.</returns>
        string GetCurrentNodeName();
    }

    internal sealed class ServiceFabricNodeContext : INodeContext
    {
        /// <inheritdoc/>
        public string GetCurrentNodeName()
        {
            return FabricRuntime.GetNodeContext().NodeName;
        }
    }
}
