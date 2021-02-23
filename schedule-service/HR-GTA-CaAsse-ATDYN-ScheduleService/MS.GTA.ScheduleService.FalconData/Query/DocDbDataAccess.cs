//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.Helper;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// Data Access for all document db connections
    /// </summary>
    public partial class DocDbDataAccess : FalconQuery, IDocDbDataAccess
    {
        private readonly IFalconQueryClient falconQueryClient;
        private readonly DocDBConfiguration configurationManager;

        /// <summary>
        /// The instance for <see cref="ILogger{DocDbDataAccess}"/>
        /// </summary>
        private readonly ILogger<DocDbDataAccess> logger;

        /// <summary>Initializes a new instance of the <see cref="DocDbDataAccess"/> class. </summary>
        /// <param name="falconQueryClient">The falcon client generator.</param>
        /// <param name="logger">Logger</param>
        public DocDbDataAccess(IFalconQueryClient falconQueryClient, ILogger<DocDbDataAccess> logger)
            : base(falconQueryClient, TraceSourceMeta.LoggerFactory.CreateLogger(nameof(FalconQuery)) as ILogger<FalconQuery>)
        {
            this.falconQueryClient = falconQueryClient;
            this.logger = logger;
            this.configurationManager = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<DocDBConfiguration>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubscriptionViewModel>> GetSystemSubscriptionViewModelByEmail(string serviceAccountEmail)
        {
            this.logger.LogInformation($"Started {nameof(this.GetSystemSubscriptionViewModelByEmail)} method in {nameof(DocDbDataAccess)}.");
            if (string.IsNullOrEmpty(serviceAccountEmail))
            {
                throw new BadRequestStatusException("GetSystemSubscriptionViewModelByEmail: Input can not be null or empty");
            }

            IEnumerable<SubscriptionViewModel> subscriptionViewModels = await this.GetSubscriptionViewModelInternal(d => d.Subscription.Resource == SchedulerConstants.MessageResource && d.ServiceAccountEmail == serviceAccountEmail);
            this.logger.LogInformation($"Finished {nameof(this.GetSystemSubscriptionViewModelByEmail)} method in {nameof(DocDbDataAccess)}.");
            return subscriptionViewModels;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubscriptionViewModel>> GetSystemSubscriptionViewModelByIds(List<string> subscrptionIds)
        {
            this.logger.LogInformation($"Started {nameof(this.GetSystemSubscriptionViewModelByIds)} method in {nameof(DocDbDataAccess)}.");
            if (subscrptionIds == null)
            {
                throw new BadRequestStatusException("GetSystemSubscriptionViewModelById: Input can not be null or empty");
            }

            IEnumerable<SubscriptionViewModel> subscriptionViewModels = await this.GetSubscriptionViewModelInternal(d => d.Subscription.Resource == SchedulerConstants.MessageResource && (d.Subscription != null) && subscrptionIds.Contains(d.Subscription.Id));
            this.logger.LogInformation($"Finished {nameof(this.GetSystemSubscriptionViewModelByIds)} method in {nameof(DocDbDataAccess)}.");
            return subscriptionViewModels;
        }

        /// <inheritdoc />
        public async Task<SubscriptionViewModel> CreateSubscriptionViewModel(SubscriptionViewModel subscription)
        {
            SubscriptionViewModel subscriptionViewModel = null;
            this.logger.LogInformation($"Started {nameof(this.CreateSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            if (subscription == null)
            {
                throw new BadRequestStatusException("CreateSubscriptionViewModel: Input can not be null or empty");
            }

            try
            {
                var client = await this.falconQueryClient.GetFalconClient(this.configurationManager.DatabaseId, this.configurationManager.IVScheduleContainerId);
                subscriptionViewModel = await client.Create(subscription);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning($"Failed to create subscription view model, Error: {ex.Message} StackTrace: {ex.StackTrace}");
            }

            this.logger.LogInformation($"Finished {nameof(this.CreateSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            return subscriptionViewModel;
        }

        /// <inheritdoc />
        public async Task<SubscriptionViewModel> UpdateSubscriptionViewModel(SubscriptionViewModel subscription)
        {
            SubscriptionViewModel subscriptionViewModel = null;
            this.logger.LogInformation($"Started {nameof(this.UpdateSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            if (subscription == null)
            {
                throw new BadRequestStatusException("UpdateSubscriptionViewModel: Input can not be null or empty");
            }

            try
            {
                var client = await this.falconQueryClient.GetFalconClient(this.configurationManager.DatabaseId, this.configurationManager.IVScheduleContainerId);
                subscriptionViewModel = await client.Update(subscription);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning($"Failed to update subscription view model, Error: {ex.Message} StackTrace: {ex.StackTrace}");
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            return subscriptionViewModel;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteSubscriptionViewModel(string subscriptionId)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new BadRequestStatusException("DeleteSubscriptionViewModel: Input can not be null or empty");
            }

            try
            {
                var client = await this.falconQueryClient.GetFalconClient(this.configurationManager.DatabaseId, this.configurationManager.IVScheduleContainerId);
                await client.Delete<SubscriptionViewModel>(subscriptionId);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning($"Failed to delete subscription view model, Error: {ex.Message} StackTrace: {ex.StackTrace}");
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteSubscriptionViewModel)} method in {nameof(DocDbDataAccess)}.");
            return true;
        }

        /// <summary>
        /// Get subscription view model
        /// </summary>
        /// <param name="expression">expression</param>
        /// <returns>Subscription view model</returns>
        private async Task<IEnumerable<SubscriptionViewModel>> GetSubscriptionViewModelInternal(Expression<Func<SubscriptionViewModel, bool>> expression)
        {
            var client = await this.falconQueryClient.GetFalconClient(this.configurationManager.DatabaseId, this.configurationManager.IVScheduleContainerId);
            return await client.Get<SubscriptionViewModel>(expression);
        }
    }
}
