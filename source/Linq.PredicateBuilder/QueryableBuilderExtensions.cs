namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// Extensions for IQueryable.
    /// </summary>
    public static class QueryableBuilderExtensions
    {
        /// <summary>
        /// Filters a sequence of entities based on a predicate builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="query">An entity sequence.</param>
        /// <param name="builder">A predicate builder.</param>
        public static IQueryable<T> FromBuilder<T>(
            [NotNull] this IQueryable<T> query,
            [NotNull] Func<QueryBuilder<T>, QueryBuilderResult<T>> builder)
        {
            return query.FromBuilder(
                builder,
                new OperationStrategy(BuilderOptions.IgnoreCase | BuilderOptions.IgnoreDefault));
        }

        /// <summary>
        /// Filters a sequence of entities based on a predicate builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="query">An entity sequence.</param>
        /// <param name="builder">A predicate builder.</param>
        /// <param name="strategy">A filtering strategy.</param>
        public static IQueryable<T> FromBuilder<T>(
            [NotNull] this IQueryable<T> query,
            [NotNull] Func<QueryBuilder<T>, QueryBuilderResult<T>> builder,
            [NotNull] IOperationStrategy strategy)
        {
            _ = builder ?? throw new ArgumentException("Builder cannot be null", nameof(builder));
            _ = strategy ?? throw new ArgumentException("Strategy cannot be null", nameof(strategy));

            var init = new QueryBuilder<T>(strategy);
            var expression = builder(init).GetExpression();

            return expression == null
                ? query
                : query.Where(expression);
        }
    }
}
