//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.WebNotifications.Interfaces
{
    using MS.GTA.Common.Base.ServiceContext;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The <see cref="IWebNotificationDataExtractor"/> interface provides mechanism to extract the web notification related property data.
    /// </summary>
    public interface IWebNotificationDataExtractor
    {
        /// <summary>
        /// Extracts the relevant properties for web notification.
        /// </summary>
        /// <returns>The instance of <see cref="Task{T}"/> with <c>T</c> as <see cref="IEnumerable{K}"/> and <c>K</c> being <see cref="Dictionary{String, String}"/>.</returns>
        Task<IEnumerable<Dictionary<string, string>>> Extract();
    }
}
