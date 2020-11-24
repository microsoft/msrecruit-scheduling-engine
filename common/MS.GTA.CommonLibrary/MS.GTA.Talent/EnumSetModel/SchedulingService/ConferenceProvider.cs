// <copyright file="ConferenceProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using System.Runtime.Serialization;

namespace MS.GTA.Talent.EnumSetModel.SchedulingService
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
