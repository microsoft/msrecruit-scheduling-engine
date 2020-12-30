// <copyright file="GraphSubscriptionManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Business
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// Graph subscription manager
    /// </summary>
    public class GraphSubscriptionManager : IGraphSubscriptionManager
    {
        private readonly IDocDbDataAccess docDbDataAccess;

        private readonly IOutlookProvider outlookProvider;

        /// <summary>
        /// The instance for <see cref="ILogger{GraphSubscriptionManager}"/>.
        /// </summary>
        private readonly ILogger<GraphSubscriptionManager> logger;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSubscriptionManager"/> class
        /// </summary>
        /// <param name="docDbDataAccess">Document db layer for all doc db opeartions</param>
        /// <param name="outlookProvider">The outlook provider instance.</param>
        /// <param name="logger">The instance for <see cref="ILogger{GraphSubscriptionManager}"/>.</param>
        /// <param name="configurationManager">The configuaration manager instance.</param>
        public GraphSubscriptionManager(
            IDocDbDataAccess docDbDataAccess,
            IOutlookProvider outlookProvider,
            ILogger<GraphSubscriptionManager> logger,
            IConfigurationManager configurationManager)
        {
            this.logger = logger;
            this.docDbDataAccess = docDbDataAccess;
            this.outlookProvider = outlookProvider;
            this.configurationManager = configurationManager;
        }

        /// <inheritdoc />
        public async Task<bool> SubscribeToInbox(string serviceAccountEmail, bool shouldRetry = true)
        {
            bool isSubscriptionUpdated = false;
            this.logger.LogInformation($"Started {nameof(this.SubscribeToInbox)} method in {nameof(GraphSubscriptionManager)}.");
            if (serviceAccountEmail == null)
            {
                throw new InvalidRequestDataValidationException("Invalid Service Account Name").EnsureLogged(this.logger);
            }

            var subscriptionViewModel = (await this.docDbDataAccess.GetSystemSubscriptionViewModelByEmail(serviceAccountEmail))?.FirstOrDefault();

            if (subscriptionViewModel == null)
            {
                this.logger.LogInformation("SubscribeToInbox(): Subscription to message cannot be found");
                subscriptionViewModel = this.GetNewSubscriptionViewModel(serviceAccountEmail);
            }
            else if (subscriptionViewModel.Subscription?.ExpirationDateTime != null && subscriptionViewModel.Subscription?.ExpirationDateTime < DateTimeOffset.Now)
            {
                await this.outlookProvider.Unsubscribe(subscriptionViewModel, serviceAccountEmail);
                await this.docDbDataAccess.DeleteSubscriptionViewModel(subscriptionViewModel.Id);

                this.logger.LogInformation($"SubscribeToInbox(): " +
                    $"Subscription {subscriptionViewModel?.Subscription?.Id} for notification url {subscriptionViewModel?.Subscription?.NotificationUrl} has been deleted due to expiration");
                subscriptionViewModel = this.GetNewSubscriptionViewModel(serviceAccountEmail);
            }

            try
            {
                var subscription = await this.outlookProvider.Subscribe(subscriptionViewModel, serviceAccountEmail, false);

                if (string.IsNullOrEmpty(subscription.Subscription?.Id))
                {
                    this.logger.LogError("SubscribeToInbox(): " +
                           $"the subscription to be created/updated has null id and therefore not creating new subscription, for email {serviceAccountEmail}");

                    isSubscriptionUpdated = false;
                }
                else if (string.IsNullOrEmpty(subscription.Id))
                {
                    var createdSubscription = await this.docDbDataAccess.CreateSubscriptionViewModel(subscription);

                    this.logger.LogInformation($"SubscribeToInbox(): " +
                        $"Successfully created message subscription {subscription.Subscription?.Id} for notification url {subscription.Subscription?.NotificationUrl} and email {serviceAccountEmail}");

                    isSubscriptionUpdated = createdSubscription != null;
                }
                else
                {
                    var updatedSubscription = await this.docDbDataAccess.UpdateSubscriptionViewModel(subscription);
                    this.logger.LogInformation($"SubscribeToInbox(): " +
                        $"Successfully updated message subscription {subscription.Subscription?.Id} for notification url {subscription.Subscription?.NotificationUrl} and email {serviceAccountEmail}");

                    isSubscriptionUpdated = updatedSubscription != null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"SubscribeToInbox(): Caught exception {ex} exception and should retry is {shouldRetry}");

                if (shouldRetry)
                {
                    if (!string.IsNullOrEmpty(subscriptionViewModel.Id))
                    {
                        this.logger.LogInformation($"SubscribeToInbox(): Removing subscription {subscriptionViewModel.Id} and graph subscription id {subscriptionViewModel.Subscription?.Id}");
                        await this.docDbDataAccess.DeleteSubscriptionViewModel(subscriptionViewModel.Id);
                    }

                    this.logger.LogInformation($"SubscribeToInbox(): Retry to subscribe to inbox for account {serviceAccountEmail}");
                    isSubscriptionUpdated = await this.SubscribeToInbox(serviceAccountEmail, shouldRetry: false);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.SubscribeToInbox)} method in {nameof(GraphSubscriptionManager)}.");
            return isSubscriptionUpdated;
        }

        private SubscriptionViewModel GetNewSubscriptionViewModel(string serviceAccountEmail, bool isSystemServiceAccount = true)
        {
            var tenantId = this.configurationManager.Get<AADClientConfiguration>().TenantID;
            var subscriptionViewModel = new SubscriptionViewModel
            {
                ServiceAccountEmail = serviceAccountEmail,
                TenantId = tenantId ?? string.Empty,
                IsSystemServiceAccount = isSystemServiceAccount
            };

            return subscriptionViewModel;
        }
    }
}
