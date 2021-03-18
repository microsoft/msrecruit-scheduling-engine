//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Tracing;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ServicePlatform.AspNetCore.Utils
{
    /// <summary>
    /// Extension utilities over <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionUtil
    {
        /// <summary>
        /// Traces all dependencies in the provided <see cref="IServiceCollection"/> using the Service Platform trace source in the following format:
        /// 
        /// "{prefix}ServiceType={dependencyServiceType}, Lifetime={dependencyLifetime}, ImplementationType={dependencyImplementationType}, Construction={dependencyConstruction}, Value={dependencyValue}"
        /// </summary>
        public static void TraceDependencies(IServiceCollection dependencies, string prefix = null)
        {
            Contract.CheckValue(dependencies, nameof(dependencies));
            Contract.CheckValueOrNull(prefix, nameof(prefix));

            TraceDependencies(ServicePlatformTrace.Instance, dependencies, prefix);
        }

        /// <summary>
        /// Traces all dependencies in the provided <see cref="IServiceCollection"/> using the provided trace source in the following format:
        /// 
        /// "{prefix}ServiceType={dependencyServiceType}, Lifetime={dependencyLifetime}, ImplementationType={dependencyImplementationType}, Construction={dependencyConstruction}, Value={dependencyValue}"
        /// </summary>
        public static void TraceDependencies<TTarceSource>(TTarceSource traceSource, IServiceCollection dependencies, string prefix = null) where TTarceSource : TraceSourceBase<TTarceSource>, new()
        {
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValue(dependencies, nameof(dependencies));
            Contract.CheckValueOrNull(prefix, nameof(prefix));

            foreach (var dependency in dependencies)
            {
                if (dependency.ImplementationInstance != null)
                {
                    traceSource.TraceInformation(
                        $"{prefix}ServiceType={dependency.ServiceType.FullName}, Lifetime={dependency.Lifetime}, ImplementationType={dependency.ImplementationInstance.GetType().FullName}, Construction=Instance, Value={dependency.ImplementationInstance.ToString()}");
                }
                else if (dependency.ImplementationType != null)
                {
                    traceSource.TraceInformation(
                        $"{prefix}ServiceType={dependency.ServiceType.FullName}, Lifetime={dependency.Lifetime}, ImplementationType={dependency.ImplementationType.FullName}, Construction=Type");
                }
                else if (dependency.ImplementationFactory != null)
                {
                    traceSource.TraceInformation(
                        $"{prefix}ServiceType={dependency.ServiceType.FullName}, Lifetime={dependency.Lifetime}, ImplementationType={dependency.ImplementationFactory.GetType().GenericTypeArguments[1].FullName}, Construction=Factory");
                }
                else
                {
                    traceSource.TraceInformation(
                        $"{prefix}ServiceType={dependency.ServiceType.FullName}, Lifetime={dependency.Lifetime}, ImplementationType=(unknown), Construction=(unknown)");
                }
            }
        }
    }
}
