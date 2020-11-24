//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ISerializer.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Calendar
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The interface for calendar file template serialization.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serialize template to calendar file format.
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="t">Any file template.</param>
        /// <returns>String representation of calendar file format.</returns>
        string Serialize<T>(T t) where T : CalendarTemplate;
    }
}
