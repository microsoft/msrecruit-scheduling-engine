//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using MS.GTA.ServicePlatform.Privacy;

namespace MS.GTA.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Represents a type used to wrap sensitive information.
    /// </summary>
    public interface IPrivateDataContainer
    {
        /// <summary>
        /// Privacy level.
        /// </summary>
        PrivacyLevel PrivacyLevel { get; }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        /// <returns>Underlying value (including sensitive information).</returns>
        object GetValue();
    }
}
