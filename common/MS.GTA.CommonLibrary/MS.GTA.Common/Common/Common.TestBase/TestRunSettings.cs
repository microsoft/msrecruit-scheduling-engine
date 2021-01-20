// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TestRunSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace MS.GTA.Common.TestBase
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>The test run settings.</summary>
    public class TestRunSettings
    {
        /// <summary>The test context.</summary>
        private readonly TestContext testContext;

        /// <summary>Initializes a new instance of the <see cref="TestRunSettings"/> class.</summary>
        /// <param name="testContext">The test context.</param>
        public TestRunSettings(TestContext testContext)
        {
            this.testContext = testContext;
        }

        /// <summary>The cluster URI.</summary>
        public Uri ClusterUri => new Uri(this.GetTestParameter("ClusterUrl"));

        /// <summary>The AAD application id.</summary>
        public string AadApplicationId => this.GetTestParameter("AadApplicationId");

        /// <summary>The AAD application key.</summary>
        public string AadApplicationKey => this.GetTestParameter("AadApplicationKey");

        /// <summary>The client certificate secret URL.</summary>
        public string ClientCertificateSecretUrl => this.GetTestParameter("ClientCertificateSecretUrl");

        /// <summary>The client certificate secret password URL.</summary>
        public string ClientCertificateSecretPwdUrl => this.GetTestParameter("ClientCertificateSecretPwdUrl");

        /// <summary>The application name.</summary>
        public string ApplicationName => this.GetTestParameter("ApplicationName");

        /// <summary>The get test parameter.</summary>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetTestParameter(string key)
        {
            if (this.testContext.Properties.ContainsKey(key))
            {
                return (string)this.testContext.Properties[key];
            }

            return Environment.GetEnvironmentVariable(key);
        }
    }
}