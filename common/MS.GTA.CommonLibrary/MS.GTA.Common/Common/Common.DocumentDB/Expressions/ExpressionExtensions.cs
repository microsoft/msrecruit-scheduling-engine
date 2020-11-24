//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExpressionExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB.Expressions
{
    using System;
    using System.Linq.Expressions;

    /// <summary>The expression extensions.</summary>
    public static class ExpressionExtensions
    {
        /// <summary>The and also.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)),
                left.Parameters);

            return combined;
        }

        /// <summary>The or else.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) 
        {
            Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)),
                left.Parameters);

            return combined;
        }
    }
}
