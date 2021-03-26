//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Security;

namespace TA.CommonLibrary.ServicePlatform.Context
{
    public sealed partial class ServiceContext
    {
        public static class Principal
        {
            /// <summary>
            /// Sets the current principal.
            /// </summary>
            /// <returns>The disposable for the new ServiceContext.</returns>
            public static IDisposable SetPrincipal(IServiceContextPrincipal principal)
            {
                var currentPrincipal = Current;
                Contract.Check(currentPrincipal == null, "Once set, the principal cannot be changed");

                return Push(principal);
            }

            internal static IServiceContextPrincipal Current
            {
                get
                {
                    var currentContext = CurrentContext;
                    if (currentContext == null)
                        return null;

                    return currentContext.principal;
                }
            }

            /// <summary>
            /// Gets the principal doing a hard cast to type of T.
            /// </summary>
            /// <exception cref="InvalidCastException">Thrown for invalid casting when the principal is not of type T.</exception>
            public static T GetCurrent<T>() where T : class, IServiceContextPrincipal
            {
                // TODO - 0000: Add custom exception for both invalid cast and for null.
                var principal = Current;

                if (principal == null)
                {
                    throw new NullReferenceException();
                }

                return (T)Current;
            }

            /// <summary>
            /// Gets the principal casting as the specified principal type.
            /// </summary>
            /// <returns>Instance of T or null when the principal is not of type T.</returns>
            public static T TryGetCurrent<T>() where T : class, IServiceContextPrincipal
            {
                return Current as T;
            }
        }
    }
}
