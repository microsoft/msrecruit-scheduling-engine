//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Represents a type used to mark sensitive information in <see cref="IPrivateDataContainer"/>.
    /// </summary>
    public interface IPrivacyMarker
    {
        /// <summary>
        /// Marks sensitive data in <paramref name="privateData"/> for later scrubbing.
        /// </summary>
        /// <param name="privateData">Instance of <see cref="IPrivateDataContainer"/> that contains value to mark.</param>
        /// <returns>Representation of <paramref name="privateData"/> with sensitive data marked.</returns>
        object ToCompliantValue(IPrivateDataContainer privateData);
    }
}
