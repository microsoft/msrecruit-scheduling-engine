//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Flighting.ECSProvider
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class InitializeProviderActivityType : SingletonActivityType<InitializeProviderActivityType>
    {
        public InitializeProviderActivityType()
            : base("SP.ECS.Initialize")
        {
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class CheckFeatureActivityType : SingletonActivityType<CheckFeatureActivityType>
    {
        public CheckFeatureActivityType()
            : base("SP.ECS.CheckFeature")
        {
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class GetEnabledFeaturesActivityType : SingletonActivityType<GetEnabledFeaturesActivityType>
    {
        public GetEnabledFeaturesActivityType() 
            : base("SP.ECS.GetEnabledFeatures")
        {
        }
    }
}
