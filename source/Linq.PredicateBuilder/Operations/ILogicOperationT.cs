namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines methods of logical operations.
    /// </summary>
    /// <typeparam name="TBuilderResult">The type of builder result.</typeparam>
    /// <typeparam name="TEntity">The type of the data in the data source.</typeparam>
    public interface ILogicOperationT<out TBuilderResult, TEntity>
    {
        /// <summary>
        /// Builds a predicate indicating whether a specified substring occurs within the string
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector expression.</param>
        /// <param name="input">The string to seek.</param>
        TBuilderResult Contains(
            [NotNull] Expression<Func<TEntity, string>> propertyExpression,
            [CanBeNull] string input);

        /// <summary>
        /// Builds a predicate indicating whether a specified input value is equal to the value
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector.</param>
        /// <param name="input">The value to compare with.</param>
        /// <typeparam name="TInput">The property and input value type.</typeparam>
        TBuilderResult Equals<TInput>(
            [NotNull] Expression<Func<TEntity, TInput>> propertyExpression,
            [CanBeNull] TInput input);

        /// <summary>
        /// Uses a specified expression as a predicate.
        /// </summary>
        /// <param name="predicate">The filtering expression.</param>
        TBuilderResult Where(
            [NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Uses a specified expression as a predicate and a specified input value as its parameter.
        /// </summary>
        /// <param name="predicate">The filtering expression.</param>
        /// <param name="input">The value to use as a parameter.</param>
        /// <typeparam name="TInput">The input value type.</typeparam>
        TBuilderResult Where<TInput>(
            [NotNull] Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input);

        /// <summary>
        /// Builds a predicate indicating whether the value defined by a property selector expression
        /// is contained in a specified collection.
        /// </summary>
        /// <param name="propertyExpression">The property selector.</param>
        /// <param name="input">The collection to check.</param>
        /// <typeparam name="TInput">The collection element type.</typeparam>
        TBuilderResult In<TInput>(
            [NotNull] Expression<Func<TEntity, TInput>> propertyExpression,
            [CanBeNull] IEnumerable<TInput> input);

        /// <summary>
        /// Builds a predicate based on filter for the nested collection property
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="propertyExpression">A collection navigation property selector.</param>
        /// <param name="builder">The nested filter builder.</param>
        /// <typeparam name="TInput">The nested collection element type.</typeparam>
        TBuilderResult Any<TInput>(
            Expression<Func<TEntity, ICollection<TInput>>> propertyExpression,
            Func<ILogicOperation<TInput>, IQueryBuilderResult<TInput>> builder);

        /// <summary>
        /// Builds a predicate using nested <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The nested builder.</param>
        TBuilderResult Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder);
    }
}