//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1.Flights
{
    using HR.TA.ServicePlatform.Flighting;

    /// <summary>
    /// The <see cref="EnableHiringManagerDelegateFlight"/> class provides a switch to enable hiring manager delegate role.
    /// </summary>
    /// <seealso cref="SingletonFlight{EnableHiringManagerDelegateFlight}" />
    public class EnableHiringManagerDelegateFlight : SingletonFlight<EnableHiringManagerDelegateFlight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnableHiringManagerDelegateFlight"/> class.
        /// </summary>
        public EnableHiringManagerDelegateFlight()
            : base("IV_HiringManagerDelegate", "2020-07-03")
        {
        }
    }
}
