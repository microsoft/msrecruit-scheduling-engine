//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.DocumentDB.Expressions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>The expression parameter replacer.</summary>
    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        /// <summary>Initializes a new instance of the <see cref="ExpressionParameterReplacer"/> class.</summary>
        /// <param name="fromParameters">The from parameters.</param>
        /// <param name="toParameters">The to parameters.</param>
        public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            this.ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();

            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
            {
                this.ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }
        }

        /// <summary>Gets the parameter replacements.</summary>
        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; }

        /// <summary>The visit parameter.</summary>
        /// <param name="node">The node.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if (this.ParameterReplacements.TryGetValue(node, out replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
