//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.ServicePlatform.Context;

namespace TA.CommonLibrary.ServicePlatform.Context
{
    public sealed class ActivityScope
    {
        public ActivityType ActivityType { get; }

        public RootExecutionContext RootExecutionContext { get; }

        public ActivityScope(ActivityType activityType)
        {
            RootExecutionContext = null;
            ActivityType = activityType;
        }

        public ActivityScope(RootExecutionContext rootExecutionContext, ActivityType activityType)
        {
            RootExecutionContext = rootExecutionContext;
            ActivityType = activityType;
        }
    }
}
