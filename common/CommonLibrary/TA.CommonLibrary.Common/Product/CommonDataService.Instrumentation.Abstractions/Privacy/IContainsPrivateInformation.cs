//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;

namespace TA.CommonLibrary.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Used for objects that contain Private Information
    /// </summary>
    [Obsolete("Use IPrivateDataContainer and IPrivacyMarker instead")]
    public interface IContainsPrivateInformation
    {
        /// <summary>
        /// Returns the tagged string representation of the object.
        /// </summary>
        /// <remarks>
        /// Caller may have multiple data field with different private data types,
        /// Service developer should implement this interface to customize
        /// their processing and transformation for each field of the object
        /// </remarks>
        string ToCompliantString();

        /// <summary>
        /// Returns the untagged string representation of the object.
        /// </summary>
        /// <remarks>
        /// Make sure to only use this method in user-facing error messages or 
        /// if it is guaranteed that the string is not leaving the trust boundary.
        /// </remarks>
        string ToOriginalString();
    }
}
