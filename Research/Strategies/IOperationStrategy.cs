namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Constructs filtering methods.
    /// </summary>
    public interface IOperationStrategy
    {
        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Contains"/>
        [CanBeNull]
        Expression<Func<TEntity, bool>> Contains<TEntity>(
            Expression<Func<TEntity, string>> propertyExpression,
            string input);

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Equals{TInput}"/>
        [CanBeNull]
        Expression<Func<TEntity, bool>> StringEquals<TEntity>(
            Expression<Func<TEntity, string>> propertyExpression,
            string input);

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Equals{TInput}"/>
        Expression<Func<TEntity, bool>> Equals<TEntity, TInput>(
            [NotNull] Expression<Func<TEntity, TInput>> propertyExpression,
            TInput input);

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.In{TInput}"/>
        [CanBeNull]
        Expression<Func<TEntity, bool>> In<TEntity, TInput>(
            [NotNull] Expression<Func<TEntity, TInput>> propertyExpression,
            [CanBeNull] IEnumerable<TInput> input);

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        [CanBeNull]
        Expression<Func<TEntity, bool>> Any<TEntity, TInput>(
            [NotNull] Expression<Func<TEntity, ICollection<TInput>>> collectionSelector,
            [CanBeNull] Expression<Func<TInput, bool>> predicate);

        /// <inheritdoc cref="ILogicOperationT{TBuilderResult,TEntity}.Where"/>
        [CanBeNull]
        Expression<Func<TEntity, bool>> Where<TEntity, TInput>(
            Expression<Func<TEntity, TInput, bool>> predicate,
            TInput input);
    }
}