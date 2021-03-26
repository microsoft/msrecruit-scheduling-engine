//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Tracing;

namespace TA.CommonLibrary.ServicePlatform.Utils
{
    /// <summary>
    /// A collection of assembly manipulation utilities.
    /// </summary>
    public static class AssemblyUtil
    {
        /// <summary>
        /// Traces all assemblies loaded in the current application domain using the Service Platform trace source in the following format:
        /// 
        /// $"{prefix}FullName={assemblyFullName}, FileVersion={assemblyFileVersion}, ProductVersion={assemblyProductVersion}, Location={assemblyLocation}"
        /// </summary>
        public static void TraceLoadedAssemblies(string prefix = null)
        {
            Contract.CheckValueOrNull(prefix, nameof(prefix));
            TraceLoadedAssemblies(ServicePlatformTrace.Instance, prefix);
        }

        /// <summary>
        /// Traces all assemblies loaded in the current application domain using the provided trace source in the following format:
        /// 
        /// $"{prefix}FullName={assemblyFullName}, FileVersion={assemblyFileVersion}, ProductVersion={assemblyProductVersion}, Location={assemblyLocation}"
        /// </summary>
        public static void TraceLoadedAssemblies<TTraceSource>(TTraceSource traceSource, string prefix = null) where TTraceSource : TraceSourceBase<TTraceSource>, new()
        {
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValueOrNull(prefix, nameof(prefix));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName);
            foreach (var assembly in assemblies)
            {
                var assemblyLocation = !assembly.IsDynamic ? assembly.Location : "(dynamic)";

                var assemblyFileVersion = "(dynamic)";
                var assemblyProductVersion = "(dynamic)";

                if (!assembly.IsDynamic)
                {
                    try
                    {
                        var assemblyVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                        assemblyFileVersion = assemblyVersionInfo?.FileVersion ?? "(null)";
                        assemblyProductVersion = assemblyVersionInfo?.ProductVersion ?? "(null)";
                    }
                    catch (FileNotFoundException)
                    {
                        // Can happen if we don't have permissions for some reason, the file got renamed, etc.
                        assemblyFileVersion = assemblyProductVersion = "(file not found)";
                    }
                }

                traceSource.TraceInformation(
                    $"{prefix}FullName={assembly.FullName}, FileVersion={assemblyFileVersion}, ProductVersion={assemblyProductVersion}, Location={assemblyLocation}");
            }
        }
    }
}
