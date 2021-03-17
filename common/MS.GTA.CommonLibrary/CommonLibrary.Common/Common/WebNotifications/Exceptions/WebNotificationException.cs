//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.WebNotifications.Exceptions
{
    using System;

    /// <summary>
    /// The <see cref="WebNotificationException"/> class indicates exception for web notifications processing.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class WebNotificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationException"/> class.
        /// </summary>
        public WebNotificationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WebNotificationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public WebNotificationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
