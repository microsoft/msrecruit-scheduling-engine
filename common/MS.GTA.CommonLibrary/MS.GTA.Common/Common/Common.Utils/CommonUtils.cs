//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CommonUtils.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------


namespace MS.GTA.Common.Utils
{
    using System;

    /// <summary>
    /// Common Utils
    /// </summary>
    public static class  CommonUtils
    {
        /// <summary>
        /// Converts string to guid.
        /// </summary>
        /// <param name="inputString">The string to convert</param>
        /// <returns>The <see cref="Guid"/> for given string.</returns>
        public static Guid ToGuid(this string inputString)
        {
            if (Guid.TryParse(inputString, out Guid guid))
            {
                return guid;
            }
            else
            {
                throw new Exception(inputString);
            }
        }

        public static Guid? ToNullableGuid(this string value)
        {
            if (!string.IsNullOrEmpty(value)
                && Guid.TryParse(value, out var guid)
                && guid != Guid.Empty)
            {
                return guid;
            }

            return null;
        }
    }
}
