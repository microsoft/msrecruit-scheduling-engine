//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1.Flights
{
    using HR.TA.ServicePlatform.Flighting;

    /// <summary>
    /// The <see cref="WebNotificationServiceFlight"/> class provides a switch to enable web notification generation.
    /// </summary>
    /// <seealso cref="SingletonFlight{WebNotificationServiceFlight}" />
    public class WebNotificationServiceFlight : SingletonFlight<WebNotificationServiceFlight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationServiceFlight"/> class.
        /// </summary>
        public WebNotificationServiceFlight()
            : base("EnableWebNotification", "2020-05-29")
        {
        }
    }
}
