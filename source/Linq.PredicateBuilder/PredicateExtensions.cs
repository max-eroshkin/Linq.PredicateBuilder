namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Predicate extensions.
    /// </summary>
    [PublicAPI]
    public static class PredicateExtensions
    {
        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        /// <param name="first">The first expression.</param>
        /// <param name="second">The second expression.</param>
        /// <typeparam name="T">Element type.</typeparam>
        public static Expression<Func<T, bool>>? And<T>(
            this Expression<Func<T, bool>>? first,
            Expression<Func<T, bool>>? second)
        {
            if (first != null && second != null)
                return first.Compose(second, Expression.AndAlso);
            else if (first == null && second == null)
                return null;
            else if (first == null)
                return second;
            else
                return first;
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        /// <param name="first">The first expression.</param>
        /// <param name="second">The second expression.</param>
        /// <typeparam name="T">Element type.</typeparam>
        public static Expression<Func<T, bool>>? Or<T>(
            this Expression<Func<T, bool>>? first,
            Expression<Func<T, bool>>? second)
        {
            if (first != null && second != null)
                return first.Compose(second, Expression.OrElse);
            if (first == null && second == null)
                return null;
            if (first == null)
                return second;
            return first;
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        /// <param name="expression">The predicate expression.</param>
        /// <typeparam name="T">Element type.</typeparam>
        public static Expression<Func<T, bool>>? Not<T>(
            this Expression<Func<T, bool>>? expression)
        {
            if (expression == null)
                return null;
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        /// <param name="first">The first expression.</param>
        /// <param name="second">The second expression.</param>
        /// <param name="merge">The merge function.</param>
        /// <typeparam name="T">Element type.</typeparam>
        private static Expression<T> Compose<T>(
            this Expression<T> first,
            Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => (Expression)p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }
}