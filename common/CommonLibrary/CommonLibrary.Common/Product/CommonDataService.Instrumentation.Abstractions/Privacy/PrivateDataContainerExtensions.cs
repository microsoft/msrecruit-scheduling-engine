//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Privacy;

namespace CommonLibrary.CommonDataService.Instrumentation.Privacy
{
    /// <summary>
    /// Extension methods to tag sensitive information in convinient way.
    /// </summary>
    public static class PrivateDataContainerExtensions
    {
        /// <summary>
        /// Wraps <paramref name="value"/> in <see cref="IPrivateDataContainer"/> with <see cref="PrivacyLevel.CustomerData"/> level.
        /// </summary>
        /// <typeparam name="T">Type of value to wrap.</typeparam>
        /// <param name="value">Value to mark as sensitive information.</param>
        /// <returns>Instance of <see cref="IPrivateDataContainer"/>.</returns>
        public static IPrivateDataContainer AsCustomerData<T>(this T value)
        {
            return new PrivateDataContainer<T>(value, PrivacyLevel.CustomerData);
        }

        /// <summary>
        /// Wraps <paramref name="value"/> in <see cref="IPrivateDataContainer"/> with <see cref="PrivacyLevel.EndUserIdentifiableInformation"/> level.
        /// </summary>
        /// <typeparam name="T">Type of value to wrap.</typeparam>
        /// <param name="value">Value to mark as sensitive information.</param>
        /// <returns>Instance of <see cref="IPrivateDataContainer"/>.</returns>
        public static IPrivateDataContainer AsEndUserIdentifiable<T>(this T value)
        {
            return new PrivateDataContainer<T>(value, PrivacyLevel.EndUserIdentifiableInformation);
        }

        /// <summary>
        /// Wraps <paramref name="value"/> in <see cref="IPrivateDataContainer"/> with <see cref="PrivacyLevel.OrganizationIdentifiableInformation"/> level.
        /// </summary>
        /// <typeparam name="T">Type of value to wrap.</typeparam>
        /// <param name="value">Value to mark as sensitive information.</param>
        /// <returns>Instance of <see cref="IPrivateDataContainer"/>.</returns>
        public static IPrivateDataContainer AsOrganizationIdentifiable<T>(this T value)
        {
            return new PrivateDataContainer<T>(value, PrivacyLevel.OrganizationIdentifiableInformation);
        }
    }
}
