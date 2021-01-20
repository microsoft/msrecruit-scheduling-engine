//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.ComponentModel;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Context
{
    public sealed partial class ServiceContext
    {
        public static partial class Environment
        {
            public static IEnvironmentContext Current
            {
                get
                {
                    var currentContext = CurrentContext;
                    if (currentContext == null || currentContext.environmentContext == null)
                        return defaultInstance;

                    return currentContext.environmentContext;
                }
            }

            private static IEnvironmentContext defaultInstance = new CloudEnvironment();

            /// <summary>
            /// Initialize the environment context
            /// </summary>
            public static void Initialize(IEnvironmentContext context)
            {
                Contract.CheckValue(context, nameof(context));

                defaultInstance = context;
            }
        }
    }
}
