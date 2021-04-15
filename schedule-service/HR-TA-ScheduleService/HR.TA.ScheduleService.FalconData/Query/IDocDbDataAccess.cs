//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.FalconData.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// DocDbDataAccess
    /// </summary>
    public interface IDocDbDataAccess
    {
        /// <summary>
        /// Create subscription view model
        /// </summary>
        /// <param name="subscription">The subscription</param>
        /// <returns>Subscription view model created</returns>
        Task<SubscriptionViewModel> CreateSubscriptionViewModel(SubscriptionViewModel subscription);

        /// <summary>
        /// Get subscription view model from system service account
        /// </summary>
        /// <param name="serviceAccountEmail">Service account</param>
        /// <returns>Subscription view model</returns>
        Task<IEnumerable<SubscriptionViewModel>> GetSystemSubscriptionViewModelByEmail(string serviceAccountEmail);

        /// <summary>
        /// Get subscription view model from subscription Id
        /// </summary>
        /// <param name="subscriptionIds">List of subscription Ids</param>
        /// <returns>Subscription view model</returns>
        Task<IEnumerable<SubscriptionViewModel>> GetSystemSubscriptionViewModelByIds(List<string> subscriptionIds);

        /// <summary>
        /// Delete subscription view model
        /// </summary>
        /// <param name="subscriptionId">Subscription Id</param>
        /// <returns>Bool result</returns>
        Task<bool> DeleteSubscriptionViewModel(string subscriptionId);

        /// <summary>
        /// Update subscription view model
        /// </summary>
        /// <param name="subscription">The subscription</param>
        /// <returns>Subscription view model updated</returns>
        Task<SubscriptionViewModel> UpdateSubscriptionViewModel(SubscriptionViewModel subscription);
    }
}
