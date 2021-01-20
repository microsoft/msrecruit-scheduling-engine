//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions
{
    /// <summary>
    /// An abstraction over internal and external web API hosts. Web API hosts are 
    /// responsible for hydrating the ServiceContext.
    /// </summary>
    public interface IWebApiHost : IStatelessHttpMiddleware
    {
    }
}
