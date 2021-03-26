//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.CommonDataService.Common.Internal
{
    /// <summary>
    /// Implement this interface for compatibility with CheckAll and AssertAll.
    /// 
    /// This interface is intended to be implemented on immutable structs to provide lightweight checking that the struct is
    /// not the default instance where the default instance is not valid.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public interface ICheckable
    {
        bool IsValid { get; }
    }
}
