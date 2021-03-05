//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ServicePlatform.AspNetCore.Http.Abstractions
{
    /// <summary>
    /// An abstraction over internal and external web API hosts. Web API hosts are 
    /// responsible for hydrating the ServiceContext.
    /// </summary>
    public interface IWebApiHost : IStatelessHttpMiddleware
    {
    }
}
