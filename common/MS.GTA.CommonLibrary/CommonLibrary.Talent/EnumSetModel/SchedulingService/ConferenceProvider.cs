//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.Talent.EnumSetModel.SchedulingService
{
    /// <summary>
    /// The <see cref="ConferenceProvider"/> enumeration has meeting provider values.
    /// </summary>
    [DataContract]
    public enum ConferenceProvider
    {
        /// <summary>
        /// The Microsoft Teams
        /// </summary>
        MicrosoftTeams = 0,

        /// <summary>
        /// The Skype
        /// </summary>
        Skype = 1
    }
}
