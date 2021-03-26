//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.ComponentModel;
using TA.CommonLibrary.CommonDataService.Common.Internal;

namespace TA.CommonLibrary.ServicePlatform.Context
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
