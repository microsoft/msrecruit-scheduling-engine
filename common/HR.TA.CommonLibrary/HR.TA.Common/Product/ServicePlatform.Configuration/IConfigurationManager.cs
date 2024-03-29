﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ServicePlatform.Configuration
{
    /// <summary>
    /// Contract for a class that can provide configuration settings.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Gets an instance of the specified configuration.
        /// </summary>
        T Get<T>() where T : class, new();

        /// <summary>
        /// Given section name and parameter name get value and cast to desired type
        /// </summary>
        T GetValue<T>(string sectionName, string parameterName);
    }
}
