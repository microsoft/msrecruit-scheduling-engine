//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Exceptions;
using MS.GTA.ServicePlatform.Tracing;
using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.Hosting
{
    internal static class ExecutionContextFactory
    {
        internal static RootExecutionContext CreateForExternalHost(string sessionIdHeader, string activityIdHeader, string activityVectorHeader, ITraceSource traceSource)
        {
            Contract.AssertValue(traceSource, nameof(traceSource));

            var executionContext = new RootExecutionContext();

            var sessionId = Guid.Empty;
            var rootActivityId = Guid.Empty;

            if (string.IsNullOrEmpty(sessionIdHeader))
            {
                sessionId = Guid.NewGuid();
                traceSource.TraceInformation(String.Format(
                    "No session is present on the request (AssignedSessionId={0})",
                    sessionId.ToString("D")));
            }
            else if (!Guid.TryParseExact(sessionIdHeader, "D", out sessionId))
            {
                sessionId = Guid.NewGuid();
                traceSource.TraceInformation(String.Format(
                    "Request session is not conforming to the expected format (OriginalSessionId={0}, AssignedSessionId={1})",
                    // sessionIdHeader.Chop(256),
                    sessionId.ToString("D")));
            }

            if (string.IsNullOrEmpty(activityIdHeader))
            {
                rootActivityId = Guid.NewGuid();
                traceSource.TraceInformation(String.Format(
                    "No activity is present on the request (AssignedActivityId={0})",
                    rootActivityId.ToString("D")));
            }
            else if (!Guid.TryParseExact(activityIdHeader, "D", out rootActivityId))
            {
                rootActivityId = Guid.NewGuid();
                traceSource.TraceInformation(String.Format(
                    "Request activity is not conforming to the expected format (OriginalActivityId={0}, AssignedActivityId={1})",
                    // activityIdHeader.Chop(256),
                    rootActivityId.ToString("D")));
            }

            executionContext.SessionId = sessionId;
            executionContext.RootActivityId = rootActivityId;
            // Extend the ActivityVector to facilitate correlation
            executionContext.ActivityVector = GetExtendedActivityVector(activityVectorHeader);

            return executionContext;
        }

        internal static RootExecutionContext TryDeserializeServiceContext(string executionContextHeader, JsonSerializer jsonSerializer, ITraceSource traceSource)
        {
            Contract.AssertValue(executionContextHeader, nameof(executionContextHeader));
            Contract.AssertValue(jsonSerializer, nameof(jsonSerializer));
            Contract.AssertValue(traceSource, nameof(traceSource));

            RootExecutionContext executionContext = null;
            if (!string.IsNullOrEmpty(executionContextHeader))
            {
                try
                {
                    executionContext = HttpJsonUtil.JsonDeserialize(jsonSerializer, executionContextHeader);
                }
                catch (FormatException ex)
                {
                    throw new InvalidExecutionContextException(ex).EnsureTraced(traceSource);
                }
                catch (JsonException ex)
                {
                    throw new InvalidExecutionContextException(ex).EnsureTraced(traceSource);
                }
            }

            if (executionContext == null)
                throw new MissingExecutionContextException().EnsureTraced(traceSource);

            return executionContext;
        }

        internal static string GetExtendedActivityVector(string currentActivityVector)
        {
            string nextActivityVector = null;
            if (!string.IsNullOrEmpty(currentActivityVector))
            {
                nextActivityVector = currentActivityVector + ".00"; // We need to extend here so that we don't lose the correlation
            }
            return nextActivityVector;
        }
    }
}
