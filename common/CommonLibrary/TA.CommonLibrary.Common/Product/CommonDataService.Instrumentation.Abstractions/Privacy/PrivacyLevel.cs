//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.ServicePlatform.Privacy
{
    /// <summary>
    /// Privacy Levels.
    /// </summary>
    /// <remarks>
    /// Ordered most to least restrictive.
    /// </remarks>
    public enum PrivacyLevel
    {
        /// <summary>
        /// Public Data : Non-Customer, Non-identifiable data.
        /// </summary>
        PublicData = 0,

        /// <summary>
        /// CustomerData : Customer data is content and information provided by the customer. 
        /// </summary>
        CustomerData = 1,

        /// <summary>
        /// EUII(End User Identifiable Information) : Information that could identify end users. 
        /// </summary>
        EndUserIdentifiableInformation = 2,

        /// <summary>
        /// OII(Organization Identifiable Information) : Information that can identify a tenant
        /// </summary>
        OrganizationIdentifiableInformation = 3,
    }
}
