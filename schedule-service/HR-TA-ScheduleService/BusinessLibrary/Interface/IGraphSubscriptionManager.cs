//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.Interface
{
    using System.Threading.Tasks;

    /// <summary>
    /// Graph subscription manager
    /// </summary>
    public interface IGraphSubscriptionManager
    {
        /// <summary>
        /// Subscribe to the scheduler message if not already subscribed to receive notifications of changes
        /// </summary>
        /// <param name="serviceAccountToken">Service account token</param>
        /// <param name="shouldRetry">shoud Retry or not</param>
        /// <returns>if the task succeeded</returns>
        Task<bool> SubscribeToInbox(string serviceAccountToken, bool shouldRetry);
    }
}
