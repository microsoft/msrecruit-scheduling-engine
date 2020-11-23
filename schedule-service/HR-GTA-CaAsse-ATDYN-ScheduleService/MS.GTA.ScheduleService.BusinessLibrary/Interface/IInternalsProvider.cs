// <copyright file="IInternalsProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using Microsoft.Graph;
    using MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate;

    /// <summary>
    /// The <see cref="IInternalsProvider"/> provides mechanism to provision internal objects.
    /// </summary>
    public interface IInternalsProvider
    {
        /// <summary>
        /// Gets the candidate communicator.
        /// </summary>
        /// <param name="requesterEmail">The requester email address.</param>
        /// <returns>The instance for <see cref="ICandidateCommunicator"/>.</returns>
        ICandidateCommunicator GetCandidateCommunicator(string requesterEmail);
    }
}
