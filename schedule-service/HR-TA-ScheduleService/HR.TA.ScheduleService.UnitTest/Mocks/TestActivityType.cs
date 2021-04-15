//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.UnitTest
{
    using HR.TA.ServicePlatform.Context;

    /// <summary>
    /// TestActivityType class
    /// </summary>
    internal class TestActivityType : SingletonActivityType<TestActivityType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestActivityType"/> class
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "StyleCop bug on nameof")]
        public TestActivityType()
            : base(nameof(TestActivityType), ActivityKind.InternalCall)
        {
        }
    }
}
