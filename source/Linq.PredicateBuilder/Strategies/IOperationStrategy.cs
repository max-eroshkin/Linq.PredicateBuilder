namespace Linq.PredicateBuilder
{
    using System.Linq.Expressions;

    /// <summary>
    /// Constructs filtering methods.
    /// </summary>
    public interface IOperationStrategy
    {
        /// <summary>
        /// Builds a predicate indicating whether a specified substring occurs within the string
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector expression.</param>
        /// <param name="input">The string to seek.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        Expression<Func<TEntity, bool>>? Contains<TEntity>(
            Expression<Func<TEntity, string>> propertyExpression,
            string? input);

        /// <summary>
        /// Builds a predicate indicating whether a specified input value is equal to the value
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector.</param>
        /// <param name="input">The value to compare with.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TInput">The property and input value type.</typeparam>
        Expression<Func<TEntity, bool>>? Equals<TEntity, TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            TInput? input);

        /// <summary>
        /// Builds a predicate indicating whether the value defined by a property selector expression
        /// is contained in a specified collection.
        /// </summary>
        /// <param name="propertyExpression">The property selector.</param>
        /// <param name="input">The collection to check.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TInput">The collection element type.</typeparam>
        Expression<Func<TEntity, bool>>? In<TEntity, TInput>(
            Expression<Func<TEntity, TInput>> propertyExpression,
            IEnumerable<TInput>? input);

        /// <summary>
        /// Builds a predicate based on filter for the nested collection property
        /// defined by a property selector expression.
        /// </summary>
        /// <param name="collectionSelector">A collection navigation property selector.</param>
        /// <param name="predicate">The predicate for the collector property.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TInput">The nested collection element type.</typeparam>
        Expression<Func<TEntity, bool>>? Any<TEntity, TInput>(
            Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
            Expression<Func<TInput, bool>>? predicate);

        /// <summary>
        /// Uses a specified expression as a predicate and a specified input value as its parameter.
        /// </summary>
        /// <param name="predicate">The filtering expression.</param>
        /// <param name="input">The value to use as a parameter.</param>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TInput">The input value type.</typeparam>
        Expression<Func<TEntity, bool>>? Where<TEntity, TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput? input);

        /// <summary>
        /// Indicates whether the builder segment should be skipped.
        /// </summary>
        /// <param name="input">The input value to check against ignoring conditions.</param>
        /// <typeparam name="TInput">The input value type.</typeparam>
        bool SegmentIgnored<TInput>(TInput input);

        /// <summary>
        /// Preprocesses (trim, lowercase if needed) the input value before building a predicate segment.
        /// </summary>
        /// <param name="input">The value to use as a parameter.</param>
        /// <typeparam name="TInput">The input value type.</typeparam>
        TInput PreprocessInput<TInput>(TInput input);

        /// <summary>
        /// Preprocesses the value of selector expression.
        /// </summary>
        /// <param name="propertyExpression">The property selector.</param>
        /// <typeparam name="TInput">The input value type.</typeparam>
        Expression PreprocessSelector<TInput>(Expression propertyExpression);
    }
}