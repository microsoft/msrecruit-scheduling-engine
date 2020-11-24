//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="MessageLeaseExpiredException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.ServicePlatform.Azure.Queue.Exceptions
{
    using System;
    using MS.GTA.ServicePlatform.Exceptions;

    public sealed class MessageLeaseExpiredException : MonitoredException
    {
        private const string ErrorMessage = "The queue message lease lock has expired.";

        public MessageLeaseExpiredException()
            : base(ErrorMessage)
        {
        }

        public MessageLeaseExpiredException(Exception innerException)
            : base(ErrorMessage, innerException)
        {
        }
    }
}
