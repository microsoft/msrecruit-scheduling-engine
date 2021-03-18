//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.CommonDataService.Common.Internal
{
    /// <summary>
    /// Contract for retrieving activity correlation
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public interface IActivityContext
    {
        /// <summary>
        /// Gets the user session id. This is most often a full browser session.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// Gets the user activity id (e.g. GUID for a specific open dashboard action)
        /// </summary>
        string RootActivityId { get; }

        /// <summary>
        /// Gets the execution activity vector.
        /// </summary>
        string ActivityVector { get; }
    }
}
