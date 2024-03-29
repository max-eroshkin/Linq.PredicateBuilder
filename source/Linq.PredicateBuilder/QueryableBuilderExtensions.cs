﻿namespace Linq.PredicateBuilder
{
    using System.Linq.Expressions;

    /// <summary>
    /// Extensions for IQueryable.
    /// </summary>
    public static class QueryableBuilderExtensions
    {
        /// <summary>
        /// Filters a sequence of entities based on a predicate builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="source">An entity sequence.</param>
        /// <param name="builder">A predicate builder.</param>
        public static IQueryable<T> Build<T>(
            this IQueryable<T> source,
            Func<IAndOrOperator<T>, IResult<T>> builder)
        {
            return source.Build(
                builder,
                null);
        }

        /// <summary>
        /// Filters a sequence of entities based on a predicate builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="source">An entity sequence.</param>
        /// <param name="builder">A predicate builder.</param>
        /// <param name="strategy">A filtering strategy.</param>
        public static IQueryable<T> Build<T>(
            this IQueryable<T> source,
            Func<IAndOrOperator<T>, IResult<T>> builder,
            IOperationStrategy? strategy)
        {
            var expression = BuildPredicate(builder, strategy);

            return expression == null
                ? source
                : source.Where(expression);
        }

        /// <summary>
        /// Filters a sequence of entities based on a predicate builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="source">An entity sequence.</param>
        /// <param name="builder">A predicate builder.</param>
        /// <param name="options">Builder options</param>
        public static IQueryable<T> Build<T>(
            this IQueryable<T> source,
            Func<IAndOrOperator<T>, IResult<T>> builder,
            BuilderOptions options)
        {
            return source.Build(
                builder,
                new OperationStrategy(options));
        }

        /// <summary>
        /// Creates a predicate based on a builder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="builder">A predicate builder.</param>
        /// <param name="strategy">A filtering strategy.</param>
        public static Expression<Func<T, bool>>? BuildPredicate<T>(
            Func<IAndOrOperator<T>, IResult<T>> builder,
            IOperationStrategy? strategy = null)
        {
            _ = builder ?? throw new ArgumentException("Builder cannot be null", nameof(builder));
            strategy ??= new OperationStrategy(BuilderOptions.Default);

            var init = new BuilderInit<T>(strategy);
            var expression = builder(init).GetExpression();

            return expression;
        }
    }
}