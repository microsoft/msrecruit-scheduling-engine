//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ServicePlatform.Azure.Queue.Exceptions
{
    using System;
    using HR.TA.ServicePlatform.Exceptions;

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
