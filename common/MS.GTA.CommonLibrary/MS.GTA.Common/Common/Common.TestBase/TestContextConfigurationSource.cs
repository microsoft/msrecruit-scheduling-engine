// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TestContextConfigurationSource.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase
{
    using System.Collections.Generic;
    using Base.Utilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class TestContextConfigurationSource : IConfigurationSource
    {
        private readonly TestContext testContext;

        public TestContextConfigurationSource(TestContext testContext)
        {
            this.testContext = testContext;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new TestContextConfigurationProvider(this.testContext);
        }
    }

    public class TestContextConfigurationProvider : ConfigurationProvider
    {
        private readonly TestContext testContext;

        public TestContextConfigurationProvider(TestContext testContext)
        {
            this.testContext = testContext;
        }
        
        public override void Load()
        {
            this.testContext.Properties.ForEach(p => Data.Add(p.Key, p.Value.ToString()));
        }
    }
}
