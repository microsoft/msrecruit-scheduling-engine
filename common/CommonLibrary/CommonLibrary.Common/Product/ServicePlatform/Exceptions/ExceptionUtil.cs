//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CommonLibrary.CommonDataService.Common.Internal;

namespace CommonLibrary.ServicePlatform.Exceptions
{
    /// <summary>
    /// Exception utility methods.
    /// </summary>
    public static class ExceptionUtil
    {
        /// <summary>
        /// Determines whether the provided <paramref name="exception"/> should be considered fatal. An exception
        /// is considered to be fatal if it, or any of the inner exceptions are one of the following type:
        /// 
        /// - System.OutOfMemoryException
        /// - System.InsufficientMemoryException
        /// - System.ThreadAbortException
        /// - System.AccessViolationException
        /// - System.SEHException
        /// - System.StackOverflowException
        /// - System.TypeInitializationException
        /// - CommonLibrary.ServicePlatform.MonitoredException marked as Fatal
        /// </summary>
        public static bool IsFatal(this Exception exception)
        {
            Contract.CheckValue(exception, nameof(exception));

            var mex = exception as MonitoredException;
            if (mex != null)
            {
                return mex.Kind == MonitoredExceptionKind.Fatal;
            }

            while (exception != null)
            {
                if ((exception is OutOfMemoryException && !(exception is InsufficientMemoryException)) ||
                (exception is ThreadAbortException) ||
                (exception is AccessViolationException) ||
                (exception is SEHException) ||
                (exception is StackOverflowException) ||
                (exception is TypeInitializationException))
                {
                    return true;
                }

                // These exceptions aren't fatal in themselves, but the CLR uses them
                // to wrap other exceptions, so we want to look deeper
                if (exception is TypeInitializationException || // TODO: May be considered fatal in itself, as cctor didn't run
                    exception is TargetInvocationException)
                {
                    exception = exception.InnerException;
                }
                else if (exception is AggregateException)
                {
                    // AggregateException can contain other AggregateExceptions in its InnerExceptions list so we 
                    // flatten it first. That will essentially create a list of exceptions from the AggregateException's 
                    // InnerExceptions property in such a way that any exception other than AggregateException is put 
                    // into this list. If there is an AggregateException then exceptions from it's InnerExceptions list are 
                    // put into this new list etc. Then a new instance of AggregateException with this flattened list is returned.
                    //
                    // AggregateException InnerExceptions list is immutable after creation and the walk happens only for 
                    // the InnerExceptions property of AggregateException and not InnerException of the specific exceptions.
                    // This means that the only way to have a circular referencing here is through reflection and forward-
                    // reference assignment which would be insane. In such case we would also run into stack overflow 
                    // when tracing out the exception since AggregateException's ToString does not have any protection there.
                    // 
                    // On that note that's another reason why we want to flatten here as opposed to just let recursion do its magic
                    // since in an unlikely case there is a circle we'll get OutOfMemory here instead of StackOverflow which is
                    // a lesser of the two evils.
                    AggregateException aex = exception as AggregateException; // Can't be null
                    AggregateException faex = aex.Flatten();
                    var iexs = faex.InnerExceptions;
                    if (iexs != null)
                    {
                        foreach (var iex in iexs)
                        {
                            if (iex.IsFatal())
                            {
                                return true;
                            }
                        }
                    }

                    exception = exception.InnerException;
                }
                else
                {
                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets an HTTP status code for the provided <paramref name="exception"/>. If the exception is an 
        /// instance of a <see cref="MonitoredException"/> that declares an HTTP status code through 
        /// <see cref="MonitoredExceptionMetadataAttribute"/> then the method returns this status code. 
        /// Otherwise the method returns 500. 
        /// </summary>
        public static int GetHttpStatusCode(this Exception exception)
        {
            Contract.CheckValue(exception, nameof(exception));

            var mex = exception as MonitoredException;
            if (mex != null)
            {
                return mex.HttpStatus;
            }

            return 500;
        }

        /// <summary>
        /// Flattens the exception and all of its inner exceptions into a single <see cref="IEnumerable{Exception}"/>. All exceptions
        /// (including AggregateException) are kept in the returned enumerable. 
        /// 
        /// The sample below demonstrates the exception hierarchy and Flatten order.
        /// </summary>
        /// <example>
        /// RootException (0)
        ///     AggregateException (1)
        ///         AggregateException (2)
        ///             LeafException (3)
        ///             LeafException (4)
        ///                 NodeException (5)
        ///         AggregateException (6)
        ///             LeafException (7)
        /// </example>
        public static IEnumerable<Exception> FlattenHierarchy(this Exception exception)
        {
            Contract.CheckValue(exception, nameof(exception));

            return FlattenHierarchyIterator(exception);
        }

        // Iterator for FlattenHierarchy
        private static IEnumerable<Exception> FlattenHierarchyIterator(Exception exception)
        {
            while (exception != null)
            {
                yield return exception;

                var aggregateException = exception as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        foreach (var flattenedInnerException in innerException.FlattenHierarchy())
                        {
                            yield return flattenedInnerException;
                        }
                    }

                    yield break;
                }
                else
                {
                    exception = exception.InnerException;
                }
            }
        }
    }
}
