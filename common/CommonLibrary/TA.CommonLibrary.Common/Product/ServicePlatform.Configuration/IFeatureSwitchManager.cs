//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.ServicePlatform.Configuration.FeatureSwitches;

namespace TA.CommonLibrary.ServicePlatform.Configuration
{
    /// <summary>
    /// Contract for a class that can provide feature switch values.
    /// </summary>
    public interface IFeatureSwitchManager
    {
        /// <summary>
        /// Given a feature get the value of the feature from configuration.
        /// </summary>
        bool IsEnabled<T>(T feature) where T : SingletonFeature<T>, new();
    }
}
