//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// Contract to be implemented by exception classes that provide a descriptive error code.
    /// </summary>
    public interface IMonitoredError
    {
        /// <summary>
        /// Gets the the error code. 
        /// Error code is a string that uniquely represents a well-known error situation within an error namespace 
        /// as defined by the owning application. (like DuplicateEntityKey, MissingEnvironment, etc.)
        /// </summary>
        string ErrorCode { get;  }

        /// <summary>
        /// Gets the namespace of the error. 
        /// An error namespace represents a set of errors that belong to a single application (like CDS.DataService, HCM.Hiring, etc.)
        /// </summary>
        string ErrorNamespace { get;  }
    }
}
