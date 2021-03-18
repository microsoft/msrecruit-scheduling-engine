//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Privacy;

namespace CommonLibrary.CommonDataService.Instrumentation.Privacy
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
