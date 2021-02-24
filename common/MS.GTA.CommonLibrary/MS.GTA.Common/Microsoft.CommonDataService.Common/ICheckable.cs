//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Implement this interface for compatibility with CheckAll and AssertAll.
    /// </summary>
    /// <remarks>
    /// This interface is intended to be implemented on immutable structs to provide lightweight checking that the struct is
    /// not the default instance where the default instance is not valid.
    /// </remarks>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    public interface ICheckable
    {
        bool IsValid { get; }
    }
}
